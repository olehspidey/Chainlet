using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chainlet.Abstraction;
using Chainlet.Chains.Abstraction;

namespace Chainlet.Chains
{
    public class Chain<TRequest> : IDependencyChain<TRequest> where TRequest : class
    {
        private readonly DependencyResolver _dependencyResolver;
        private readonly ICollection<DependencyHandlerBase<TRequest>> _handlers;

        public Chain(DependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver ?? throw new ArgumentNullException(nameof(dependencyResolver));
            _handlers = new List<DependencyHandlerBase<TRequest>>();
        }

        public static IDependencyChain<TRequest> Setup(DependencyResolver dependencyResolver)
            => new Chain<TRequest>(dependencyResolver);

        public IDependencyChain<TRequest> Pin<THandler>() where THandler : DependencyHandlerBase<TRequest>
        {
            var handlerType = typeof(THandler);

            if (handlerType.IsAbstract)
                throw new NotSupportedException("Handler should not be abstract");

            var handler = _dependencyResolver(handlerType);

            if (handler == null)
                throw new ApplicationException($"Handler {handlerType} was not found in DI container");
            if (!(handler is THandler))
                throw new NotSupportedException($"DI container returned invalid dependency {handler.GetType()}");

            _handlers.Add((THandler)handler);

            return this;
        }

        public async Task ProcessAsync(TRequest request)
        {
            var firstHandler = _handlers.FirstOrDefault();

            if (_handlers.Count == 1)
            {
                // ReSharper disable once PossibleNullReferenceException
                firstHandler.DependencyResolver = _dependencyResolver;
                await firstHandler.HandleAsync(request);

                return;
            }

            _handlers.Aggregate((prev, next) =>
            {
                prev.Next = next;
                prev.DependencyResolver = _dependencyResolver;
                next.DependencyResolver = _dependencyResolver;

                return next;
            });

            if (firstHandler != null)
                await firstHandler.HandleAsync(request);
        }
    }
}