namespace SeungYongShim.Proto.OpenTelemetry.Kafka
{
    public record SendMessage(string topic, IMessage messageDto);
}
