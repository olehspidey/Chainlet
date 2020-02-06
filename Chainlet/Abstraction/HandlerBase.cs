using System.Threading.Tasks;

namespace Chainlet.Abstraction
{
    public abstract class HandlerBase<TRequest> : IHandler<TRequest>
    {
        public IHandler<TRequest> Next { get; set; }

        public abstract Task HandleAsync(TRequest request);

        protected void InterruptNextHandlers()
            => Next = null;

        protected virtual bool CanHandle(TRequest request)
            => true;

        protected abstract Task HandleRequestAsync(TRequest request);
    }
}