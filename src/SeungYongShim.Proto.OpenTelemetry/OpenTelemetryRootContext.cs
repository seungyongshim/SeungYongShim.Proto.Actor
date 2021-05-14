using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Proto;

namespace Nexon.MessageTeam.Proto.OpenTelemetry
{
    public static class Middleware
    {
        public static Func<Sender, Sender> OpenTelemetrySenderMiddleware()
            => next => async (context, target, envelope) =>
            {
                Task SimpleNext() => next(context, target, envelope);

                envelope = envelope.WithHeader("ActivityId", Activity.Current?.Id);

                await SimpleNext().ConfigureAwait(false);
            };
    }
}
