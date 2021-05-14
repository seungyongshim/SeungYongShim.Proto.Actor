using System.Diagnostics;

namespace Nexon.MessageTeam.Proto.OpenTelemetry
{
    public static class ActivitySourceStatic
    {
        public static ActivitySource Instance { get; } = new ActivitySource("Nexon.MessageTeam.Proto.OpenTelemetry");
    }
}
