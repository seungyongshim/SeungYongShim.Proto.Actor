using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nexon.MessageTeam.Proto.OpenTelemetry;
using Proto;
using Proto.DependencyInjection;

namespace SampleWebApp.Actors
{
    public class HelloManagerActor : IActor
    {
        public HelloManagerActor(ILogger<HelloManagerActor> logger)
        {
            Logger = logger;
        }

        public ILogger<HelloManagerActor> Logger { get; }

        public Task ReceiveAsync(IContext context) => context.Message switch
        {
            string msg => OnHelloMessage(msg, context),
            _ => Task.CompletedTask
        };

        public async Task OnHelloMessage(string msg, IContext context)
        {
            var activity = Activity.Current;
            Logger.LogInformation(msg);
            context.Respond("Hello");

            var worker = context.Spawn(context.PropsFactory<HelloWorkerActor>()
                                              .Create());

            context.Send(worker, "World");

            await Task.CompletedTask;
        }
    }
}
