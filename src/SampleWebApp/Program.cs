using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Trace;
using Proto;
using SampleWebApp.Actors;
using Serilog;
using Serilog.Enrichers.ActivityTags;
using Serilog.Enrichers.Span;

namespace SampleWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                       webBuilder.UseStartup<Startup>();
                   })
                   .ConfigureServices(services =>
                   {
                       services.AddOpenTelemetryTracing(builder => builder
                               .AddSource("SeungYongShim.OpenTelemetry")
                               .AddAspNetCoreInstrumentation()
                               .AddJaegerExporter()
                               .SetSampler(new AlwaysOnSampler()));
                   })
                   .UseProtoActor(_ => _, _ => _, root =>
                   {
                       var props = root.SpawnNamed(root.PropsFactory<HelloManagerActor>().Create(),
                                                   "HelloManage");
                   })
                   .UseProtoActorOpenTelemetry()
                   .UseSerilog((context, config) => config
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.With<ActivityEnricher>()
                    .Enrich.With<ActivityTagsEnricher>());
    }
}
