using System.Threading.Tasks;

namespace Chainlet.Chains.Abstraction
{
    public interface IProcessable<in TRequest>
    {
        Task ProcessAsync(TRequest request);
    }
}