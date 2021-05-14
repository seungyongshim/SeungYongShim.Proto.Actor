using System;
using Microsoft.Extensions.DependencyInjection;
using Nexon.MessageTeam.Proto.OpenTelemetry;
using Proto;
using SeungYongShim.Proto.DependencyInjection;

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
            => Props.FromProducer(() => (T)ActivatorUtilities.CreateInstance(ServiceProvider, typeof(T), args))
                    .WithOpenTelemetry();
    }
    
}
