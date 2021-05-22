using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Proto;
using SeungYongShim.Kafka;

namespace SeungYongShim.Proto.OpenTelemetry.Kafka
{
    public partial class KafkaConsumerActor : IActor
    {
        public KafkaConsumerActor(ILogger<KafkaConsumerActor> logger,
                                  KafkaConsumer kafkaConsumer,
                                  ICollection<string> topics,
                                  string groupId,
                                  PID parserActor)
        {
            Logger = logger;
            KafkaConsumer = kafkaConsumer;
            Topics = topics;
            GroupId = groupId;
            ParserActor = parserActor;
            Logger.LogInformation("Create KafkaConsumerActor");
        }

        public ILogger<KafkaConsumerActor> Logger { get; }
        public KafkaConsumer KafkaConsumer { get; }
        public ICollection<string> Topics { get; }
        public string GroupId { get; }
        public PID ParserActor { get; }

        public Task ReceiveAsync(IContext context) => context.Message switch
        {
            Restarting msg => Handle(msg),
            Stopped msg => Handle(msg),
            Started msg => Handle(msg, context),
            KafkaRequest msg => Handle(msg, context),
            _ => Task.CompletedTask
        };

        private Task Handle(Restarting msg)
        {
            KafkaConsumer.Stop();
            KafkaConsumer.Dispose();
            return Task.CompletedTask;
        }

        private async Task Handle(KafkaRequest msg, IContext context)
        {
            context.Send(context.Self, KafkaRequest.Instance);
            var receive = await KafkaConsumer.ConsumeAsync(TimeSpan.FromMinutes(1));
            using var activity = ActivitySourceStatic.Instance.StartActivity("KafkaConsumerActor", ActivityKind.Internal, receive.ActivityId);

            _ = await context.RequestAsync<KafkaCommit>(ParserActor, receive.Message);
            receive.Commit();
        }

        private Task Handle(Stopped msg)
        {
            KafkaConsumer.Stop();
            KafkaConsumer.Dispose();
            return Task.CompletedTask;
        }

        private Task Handle(Started msg, IContext context)
        {
            context.Send(context.Self, KafkaRequest.Instance);
            KafkaConsumer.Start(GroupId, Topics);
            return Task.CompletedTask;
        }
    }
}
