using System;
using System.Threading.Tasks;
using Proto;
using SampleWebApp.Messages;

namespace SampleWebApp.Actors
{
    public class HelloWorkerActor : IActor
    {
        public Task ReceiveAsync(IContext context) => context.Message switch
        {
            Hello a => throw new Exception("Crash!!!"),
            _ => Task.CompletedTask
        };
    }
}
