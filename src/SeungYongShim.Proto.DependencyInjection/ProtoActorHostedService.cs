using System;
using System.Threading;
using System.Threading.Tasks;
using Proto;

namespace Microsoft.Extensions.Hosting
{
    public delegate void ProtoActorHostedServiceStart(IRootContext root);

    internal class ProtoActorHostedService : IHostedService
    {
        public ProtoActorHostedService(IServiceProvider serviceProvider,
                                       IRootContext root,
                                       ProtoActorHostedServiceStart protoActorHostedServiceStart)
        {
            ServiceProvider = serviceProvider;
            Root = root;
            ProtoActorHostedServiceStart = protoActorHostedServiceStart;
        }

        public IServiceProvider ServiceProvider { get; }
        public IRootContext Root { get; }
        public ProtoActorHostedServiceStart ProtoActorHostedServiceStart { get; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            ProtoActorHostedServiceStart(Root);

            await Task.Delay(300);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Root.System.ShutdownAsync();
        }
    }
}
