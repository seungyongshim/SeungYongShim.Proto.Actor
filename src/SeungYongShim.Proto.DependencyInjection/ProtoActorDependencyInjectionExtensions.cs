using System;
using Microsoft.Extensions.DependencyInjection;
using Proto;
using SeungYongShim.Proto.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
    public static class ProtoActorDependencyInjectionExtensions
    {
        public static IHostBuilder UseProtoActor(this IHostBuilder host,
                                                 Func<ActorSystemConfig, ActorSystemConfig> configFunc,
                                                 ProtoActorHostedServiceStart akkaHostedServiceStart)
        {
            host.ConfigureServices((context, services) =>
            {
                services.AddSingleton(akkaHostedServiceStart);
                services.AddSingleton(sp => new ActorSystem(configFunc?.Invoke(ActorSystemConfig.Setup()))
                                               .WithServiceProvider(sp));
                services.AddSingleton(typeof(IPropsFactory<>), typeof(PropsFactory<>));
                services.AddHostedService<ProtoActorHostedService>();
                services.AddSingleton(sp => (IRootContext)new RootContext(sp.GetService<ActorSystem>()));
            });

            return host;
        }
    }
}
