using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chainlet.Abstraction;

namespace Chainlet.Chains
{
    public class SimpleChain<TRequest> : IChain<TRequest>
    {
        private readonly ICollection<IHandler<TRequest>> _handlers;

        public static IChain<TRequest> Empty => new SimpleChain<TRequest>();

        public SimpleChain()
        {
            _handlers = new HashSet<IHandler<TRequest>>();
        }

        public IChain<TRequest> Pin(SimpleHandlerBase<TRequest> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            _handlers.Add(handler);

            return this;
        }

        public Task ProcessAsync(TRequest request)
        {
            _handlers.Aggregate((prev, next) =>
            {
                prev.Next = next;

                return next;
            });

            return _handlers.FirstOrDefault()?.HandleAsync(request);
        }
    }
}