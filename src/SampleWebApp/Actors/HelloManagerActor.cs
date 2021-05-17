using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Proto;
using SampleWebApp.Messages;

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
            Hello msg => OnHelloMessage(msg, context),
            _ => Task.CompletedTask
        };

        public async Task OnHelloMessage(Hello msg, IContext context)
        {
            Logger.LogInformation(msg.ToString());
            context.Respond("Hello");

            var worker = context.Spawn(context.PropsFactory<HelloWorkerActor>()
                                              .Create());

            context.Send(worker, msg);

            await Task.CompletedTask;
        }
    }
}
