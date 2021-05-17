using System;
using System.Diagnostics;
using System.Threading.Tasks;
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
                var message = envelope.Message;

                using var activity = ActivitySourceStatic.Instance.StartActivity($"{pid}@{message.GetType().Name}",
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
            Func<PID> ret = Activity.Current?.Id switch
            {
                null => () => base.SpawnNamed(props, name),
                _ => () =>
                {
                    using var activity = ActivitySourceStatic.Instance.StartActivity(string.Empty);
                    var pid = base.SpawnNamed(props, name);

                    activity.DisplayName = $"{pid}@SpawnActor";
                    return pid;
                }
            };

            return ret.Invoke();
        }
    }
}
