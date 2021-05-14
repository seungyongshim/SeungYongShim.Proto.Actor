using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Proto;
using Microsoft.Extensions.Logging;

namespace SampleConsoleApp
{
    internal class PongActor : IActor
    {
        public ILogger<PongActor> Logger { get; }
        public string World { get; }

        public PongActor(ILogger<PongActor> logger,
                         string world)
        {
            Logger = logger;
            World = world;
        }

        public Task ReceiveAsync(IContext context) => context.Message switch
        {
            string a => OnReceiveString(a),
            _ => Task.CompletedTask
        };
        private async Task OnReceiveString(string hello)
        {
            Console.WriteLine($"{hello} {World}");
            Logger.LogInformation($"{hello} {World}");

            await Task.CompletedTask;
        }
    }


    class Program
    {
        static async Task Main(string[] args)
        {
            await
            Host.CreateDefaultBuilder()
                .UseProtoActor(ActorSystemConfig.Setup(), root =>
                {
                    var pongActor = root.SpawnNamed(root.PropsFactory<PongActor>().Create("World"),
                                                    "PongActor");

                    root.Send(pongActor, "Hello");
                })
                .RunConsoleAsync();
        }
    }
}
