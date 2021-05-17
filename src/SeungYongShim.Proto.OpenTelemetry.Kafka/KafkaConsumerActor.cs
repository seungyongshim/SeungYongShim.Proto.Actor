using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proto;
using SeungYongShim.Kafka;

namespace SeungYongShim.Proto.OpenTelemetry.Kafka
{
    public class KafkaConsumerActor : IActor
    {
        public KafkaConsumerActor(KafkaConsumer kafkaConsumer,
                                  ICollection<string> topics,
                                  string groupId,
                                  PID parserActor)
        {
            KafkaConsumer = kafkaConsumer;
            Topics = topics;
            GroupId = groupId;
            ParserActor = parserActor;
        }

        public Action<Action<Commitable>> RunKafkaConsumer { get; }
        public KafkaConsumer KafkaConsumer { get; }
        public ICollection<string> Topics { get; }
        public string GroupId { get; }
        public PID ParserActor { get; }

        public Task ReceiveAsync(IContext context) => context.Message switch
        {
            Stopped msg => Handle(msg),
            Started msg => Handle(msg, context),
            Commitable msg => Handle(msg, context),
            _ => Task.CompletedTask
        };

        private async Task Handle(Commitable msg, IContext context)
        {
            await context.RequestAsync<KafkaCommit>(ParserActor, msg);
            msg.Commit();
        }

        private Task Handle(Stopped msg)
        {
            KafkaConsumer.Stop();
            KafkaConsumer.Dispose();
            return Task.CompletedTask;
        }

        private Task Handle(Started msg, IContext context)
        {
            KafkaConsumer.Run(GroupId, Topics, m => context.Send(context.Self, m));
            return Task.CompletedTask;
        }
    }
}
