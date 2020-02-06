using System.Threading.Tasks;

namespace Chainlet.Abstraction
{
    public interface IHandler<TRequest>
    {
        IHandler<TRequest> Next { get; set; }

        Task HandleAsync(TRequest request);
    }
}