using System;
using System.Threading.Tasks;
using Chainlet.Abstraction;

namespace Chainlet.Pipes.Abstraction
{
    public interface IChainPipe<out THandler, TRequest> where THandler : DependencyHandlerBase<TRequest>
    {
        Task HandleAsync(Func<TRequest, Task> next, TRequest request);
    }
}