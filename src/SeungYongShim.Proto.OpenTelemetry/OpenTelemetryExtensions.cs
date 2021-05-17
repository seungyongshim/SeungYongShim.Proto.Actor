using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Proto;

namespace SeungYongShim.Proto.OpenTelemetry
{
    public static class OpenTelemetryExtensions
    {
        public static Props WithOpenTelemetry(this Props props) =>
            props.WithContextDecorator(ctx => new OpenTelemetryActorContextDecorator(ctx))
                 .WithSenderMiddleware(OpenTelemetrySenderMiddleware());

        internal static Func<Sender, Sender> OpenTelemetrySenderMiddleware()
            => next => async (context, target, envelope) =>
            {
                Task SimpleNext() => next(context, target, envelope);

                envelope = envelope.WithHeader("ActivityId", Activity.Current?.Id);

                await SimpleNext().ConfigureAwait(false);
            };
    }
}
