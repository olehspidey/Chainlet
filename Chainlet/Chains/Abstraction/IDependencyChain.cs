using Chainlet.Abstraction;

namespace Chainlet.Chains.Abstraction
{
    public interface IDependencyChain<TRequest> : IProcessable<TRequest> where TRequest : class
    {
        IDependencyChain<TRequest> Pin<THandler>() where THandler : DependencyHandlerBase<TRequest>;
    }
}