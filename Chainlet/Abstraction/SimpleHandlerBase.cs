using System.Threading.Tasks;

namespace Chainlet.Abstraction
{
    public abstract class SimpleHandlerBase<TRequest> : HandlerBase<TRequest>
    {
        public override async Task HandleAsync(TRequest request)
        {
            if (CanHandle(request))
                await HandleRequestAsync(request);

            if (Next != null)
                await Next.HandleAsync(request);
        }
    }
}