using Chainlet.Chains.Abstraction;

namespace Chainlet.Abstraction
{
    public interface IChain<TRequest> : IProcessable<TRequest>
    {
        IChain<TRequest> Pin(SimpleHandlerBase<TRequest> hadler);
    }
}