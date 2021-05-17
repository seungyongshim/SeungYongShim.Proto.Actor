using System;
using Google.Protobuf;

namespace SeungYongShim.Proto.OpenTelemetry.Kafka
{
    public record SendMessage(string Topic, IMessage Dto, string Key)
    {
        public record Result();
        public record ResultException(Exception ex) : Result;
    }
}


