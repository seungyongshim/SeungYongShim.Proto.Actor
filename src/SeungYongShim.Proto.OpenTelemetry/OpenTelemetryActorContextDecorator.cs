using System;                                                                
using System.Diagnostics;
using System.Threading.Tasks;
using Nexon.MessageTeam.Proto.OpenTelemetry;
using Proto;

namespace SeungYongShim.Proto.OpenTelemetry
{
    internal class OpenTelemetryActorContextDecorator : ActorContextDecorator
    {
        public IContext Context { get; }

        public OpenTelemetryActorContextDecorator(IContext context) : base(context)
            => Context = context;

        public override async Task Receive(MessageEnvelope envelope)
        {
            var activityId = envelope.Header.GetOrDefault("ActivityId");
            if (activityId != null)
            {
                var pid = Context.Self;
                var message = envelope.Message.ToString();

                using var activity = ActivitySourceStatic.Instance.StartActivity($"{pid}@{message}",
                                                                                 ActivityKind.Internal,
                                                                                 activityId);
                activity.AddTag("Actor.Path", pid);
                activity.AddTag("Actor.Message", message);

                try
                {
                    await base.Receive(envelope);
                }
                catch (Exception ex)
                {
                    using var exceptionActivity = ActivitySourceStatic.Instance.StartActivity($"Exception");

                    exceptionActivity.RecordException(ex);
                    exceptionActivity.SetError();

                    throw;
                }
            }
            else
            {
                await base.Receive(envelope);
            }
        }

        public override PID SpawnNamed(Props props, string name)
        {
            var activityId = Activity.Current?.Id;
            if (activityId != null)
            {
                using var activity = ActivitySourceStatic.Instance.StartActivity("");
                var ret = base.SpawnNamed(props, name);

                activity.DisplayName = $"{ret}@SpawnActor";
                return ret;
            }
            else
            {
                return base.SpawnNamed(props, name);
            }
        }
    }
}
