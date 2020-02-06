using System.Threading.Tasks;
using Chainlet.Abstraction;

namespace Chainlet.Tests.TestFixtures
{
    public class SimpleTestFixture<TRequest>
    {
        public Handler1 HandlerFirst { get; } = new Handler1();
        public Handler2 HandlerSecond { get; } = new Handler2();

        public class Handler1 : SimpleHandlerBase<TRequest>
        {
            protected override Task HandleRequestAsync(TRequest request)
            {
                return Task.CompletedTask;
            }
        }

        public class Handler2 : SimpleHandlerBase<TRequest>
        {
            protected override Task HandleRequestAsync(TRequest request)
            {
                return Task.CompletedTask;
            }
        }
    }
}