using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Proto;
using Proto.Remote.
using Microsoft.Extensions.Logging;
using Proto.Remote.GrpcCore;

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
            Logger.LogInformation($"{hello} {World}");

            await Task.CompletedTask;
        }
    }


    class Program
    {
        static async Task Main(string[] args)
        {
            Host.CreateDefaultBuilder()
                .UseProtoActor(config => config, sys => sys,
                root =>
                {
                    var pongActor = root.SpawnNamed(root.PropsFactory<PongActor>().Create("World"),
                                                    "PongActor");

                    root.Send(pongActor, "Hello");
                })
                .RunConsoleAsync();
        }
    }
}
