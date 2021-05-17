using System;
using Google.Protobuf;

namespace SeungYongShim.Proto.OpenTelemetry.Kafka
{
    public record KafkaMessage(string Topic, IMessage Dto, string Key)
    {
        public KafkaMessage(string topic, IMessage dto) : this(topic, dto, string.Empty)
        {
        }

        public record Result();
        public record ResultException(Exception Exception) : Result;
    }
}
