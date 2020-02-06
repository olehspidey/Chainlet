using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Chainlet.Abstraction;
using Chainlet.Chains;
using Chainlet.Pipes.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace Chainlet.TestConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            var stopWatch = new Stopwatch();

            serviceCollection.AddTransient<Handle2>();
            serviceCollection.AddTransient<Handler3>();
            serviceCollection.AddTransient<Service>();
            //serviceCollection.AddTransient(typeof(Pipe));
            //serviceCollection.AddTransient(typeof(IChainPipe<Handle2, string>), typeof(Pipe2));
            var provider = serviceCollection.BuildServiceProvider();

            var dependencyChain = Chain<string>
                .Setup(provider.GetService)
                .Pin<Handle2>()
                .Pin<Handler3>();
            stopWatch.Start();

            //for (int i = 0; i < 1000_000; i++)
            {
                await dependencyChain.ProcessAsync("Hello");
            }
            stopWatch.Stop();

            Console.WriteLine($"Elapsed {stopWatch.Elapsed}");
            //await SimpleChain<string>
            //    .Empty
            //    .Pin(new Handler1())
            //    .Pin(new Handler3())
            //    .ProcessAsync("Test");
        }
    }

    class Pipe : IChainPipe<Handler3, string>
    {
        public async Task HandleAsync(Func<string, Task> next, string request)
        {
            Console.WriteLine($"Starting process requestt: {request}");
            await next(request);
            Console.WriteLine($"End process request: {request}");
        }
    }

    class Pipe2 : IChainPipe<Handle2, string>
    {
        public async Task HandleAsync(Func<string, Task> next, string request)
        {
            Console.WriteLine($"Starting process request: {request}");
            await next(request);
            Console.WriteLine($"End process request: {request}");
        }
    }

    class Handler1 : SimpleHandlerBase<string>
    {
        protected override Task HandleRequestAsync(string request)
        {
            Console.WriteLine(nameof(Handler1) + request);
            var res = nameof(Handler1) + request;

            return Task.CompletedTask;
        }
    }

    class Handle2 : DependencyHandlerBase<string>
    {
        public Handle2(Service service)
        {

        }

        protected override Task HandleRequestAsync(string request)
        {
            Console.WriteLine(nameof(Handle2) + request);
            //InterruptNextHandlers();
            var res = nameof(Handler1) + request;
            
            return Task.CompletedTask;
        }
    }

    class Handler3 : DependencyHandlerBase<string>
    {
        protected override Task HandleRequestAsync(string request)
        {
            Console.WriteLine(nameof(Handler3) + request);
            var res = nameof(Handler1) + request;

            return Task.CompletedTask;
        }
    }

    class Service
    {

    }
}
