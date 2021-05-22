using System.Diagnostics;

namespace SeungYongShim.Proto.OpenTelemetry.Kafka
{
    internal static class ActivitySourceStatic
    {
        internal static ActivitySource Instance { get; } = new ActivitySource("SeungYongShim.OpenTelemetry");
    }
}
