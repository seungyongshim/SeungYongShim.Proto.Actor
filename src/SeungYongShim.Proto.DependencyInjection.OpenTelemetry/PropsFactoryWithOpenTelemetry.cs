using System;
using Microsoft.Extensions.DependencyInjection;
using Proto;
using SeungYongShim.Proto.DependencyInjection;
using SeungYongShim.Proto.OpenTelemetry;

namespace Microsoft.Extensions.Hosting
{
    internal class PropsFactoryWithOpenTelemetry<T> : IPropsFactory<T> where T : IActor
    {
        public PropsFactoryWithOpenTelemetry(IRootContext rootContext, IServiceProvider serviceProvider)
        {
            RootContext = rootContext;
            ServiceProvider = serviceProvider;
        }

        public IRootContext RootContext { get; }
        public IServiceProvider ServiceProvider { get; }

        public Props Create(params object[] args)
            => Props.FromProducer(() => ServiceProvider.CreateInstance<T>(args))
                    .WithOpenTelemetry()
                    .WithContextDecorator(ctx => new LoggerActorContextDecorator(ctx, ServiceProvider));
    }
    
}
