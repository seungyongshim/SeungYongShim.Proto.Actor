using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Proto;
using SeungYongShim.Kafka;

namespace SeungYongShim.Proto.OpenTelemetry.Kafka
{
    public class KafkaProducerActor : IActor
    {
        public KafkaProducerActor(ILogger<KafkaProducerActor> logger,
                                  KafkaProducer producer,
                                  string persistenceId)
        {
            Logger = logger;
            Producer = producer;
        }

        public ILogger<KafkaProducerActor> Logger { get; }
        public KafkaProducer Producer { get; }

        public Task ReceiveAsync(IContext context) => context.Message switch
        {
            Stopped msg => Handle(msg),
            SendMessage msg => Handle(msg, context.Sender, context),
            _ => Task.CompletedTask,
        };

        private Task Handle(Stopped _)
        {
            Producer.Dispose();
            return Task.CompletedTask;
        }

        private async Task Handle(SendMessage msg, PID sender, IContext context)
        {
            try
            {
                await Producer.SendAsync(msg.Dto, msg.Topic, msg.Key);
                context.Send(sender, new SendMessage.Result());
            }
            catch (Exception ex)
            {
                context.Send(sender, new SendMessage.ResultException(ex));
            }
        }
    }
}
