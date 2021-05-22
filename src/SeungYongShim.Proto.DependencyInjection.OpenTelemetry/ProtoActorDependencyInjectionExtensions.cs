using Microsoft.Extensions.DependencyInjection;
using Proto;
using SeungYongShim.Proto.DependencyInjection;
using SeungYongShim.Proto.OpenTelemetry;
using static SeungYongShim.Proto.OpenTelemetry.Middleware;

namespace Microsoft.Extensions.Hosting
{
    public static class ProtoActorDependencyInjectionExtensions
    {
        public static IHostBuilder UseProtoActorOpenTelemetry(this IHostBuilder host)
        {
            host.ConfigureServices((_, services) =>
            {
                services.AddOpenTelemetry();
            });
            return host;
        }

        internal static IServiceCollection AddOpenTelemetry(this IServiceCollection services)
        {
            services.AddSingleton(sp => (IRootContext)new RootContext(sp.GetService<ActorSystem>())
                    .WithSenderMiddleware(OpenTelemetrySenderMiddleware()));
            services.AddSingleton(typeof(IPropsFactory<>), typeof(PropsFactoryWithOpenTelemetry<>));
            return services;
        }
    }
}
