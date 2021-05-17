using System.Diagnostics;

namespace SeungYongShim.Proto.OpenTelemetry
{
    public static class ActivitySourceStatic
    {
        public static ActivitySource Instance { get; } = new ActivitySource("SeungYongShim.Proto.OpenTelemetry");
    }
}
