using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Chainlet.Pipes.Abstraction;

namespace Chainlet.Abstraction
{
    public abstract class DependencyHandlerBase<TRequest> : HandlerBase<TRequest>
    {
        public DependencyResolver DependencyResolver { get; set; }

        private readonly IDictionary<Type, DependencyDescriptor> _pipesCache = new Dictionary<Type, DependencyDescriptor>();

        public override async Task HandleAsync(TRequest request)
        {
            if (CanHandle(request))
            {
                var currentType = GetType();
                var assembly = Assembly.GetAssembly(currentType);
                var pipeDependencyDescriptor = FindPipe(assembly, currentType);

                if (pipeDependencyDescriptor.ImplementationType != null && DependencyResolver(pipeDependencyDescriptor.ImplementationType) is IChainPipe<DependencyHandlerBase<TRequest>, TRequest> directlyInjectedPipe)
                {
                    await directlyInjectedPipe.HandleAsync(HandleRequestAsync, request);

                    if (Next != null)
                        await Next.HandleAsync(request);

                    return;
                }

                if (pipeDependencyDescriptor.DependencyType != null && DependencyResolver(pipeDependencyDescriptor.DependencyType) is IChainPipe<DependencyHandlerBase<TRequest>, TRequest> interInjectedPipe)
                {
                    await interInjectedPipe.HandleAsync(HandleRequestAsync, request);

                    if (Next != null)
                        await Next.HandleAsync(request);

                    return;
                }

                await HandleRequestAsync(request);

                if (Next != null)
                    await Next.HandleAsync(request);
            }
        }

        private DependencyDescriptor FindPipe(Assembly assembly, Type handlerType)
        {
            if (_pipesCache.ContainsKey(handlerType))
                return _pipesCache[handlerType];

            Type interfaceType = null;

            var implType = assembly.GetTypes().FirstOrDefault(type =>
            {
                if (type.IsAbstract)
                    return false;

                var typeInterfaces = type.GetInterfaces();

                interfaceType = typeInterfaces.FirstOrDefault(@interface =>
                {
                    var ht = @interface.GenericTypeArguments.FirstOrDefault();

                    return ht != null
                           && ht == handlerType
                           && @interface.GenericTypeArguments.Skip(1).FirstOrDefault() == typeof(TRequest);
                });

                return interfaceType != null;
            });
            var dependencyDescriptor = new DependencyDescriptor(interfaceType, implType);

            _pipesCache[handlerType] = dependencyDescriptor;

            return dependencyDescriptor;
        }
    }
}