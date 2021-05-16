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
                                  KafkaProducer producer)
        {
            Logger = logger;
            Producer = producer;
        }

        public ILogger<KafkaProducerActor> Logger { get; }
        public KafkaProducer Producer { get; }

        public Task ReceiveAsync(IContext context) => context.Message switch
        {
            _ => throw new NotImplementedException()
        };
    }
}
