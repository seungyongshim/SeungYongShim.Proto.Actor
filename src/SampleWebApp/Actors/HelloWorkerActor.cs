using System;
using System.Threading.Tasks;
using Proto;

namespace SampleWebApp.Actors
{
    public class HelloWorkerActor : IActor
    {
        public Task ReceiveAsync(IContext context) => context.Message switch
        {
            string a => throw new Exception("Crash!!!"),
            _ => Task.CompletedTask
        };
    }
}
