namespace SeungYongShim.Proto.OpenTelemetry.Kafka
{
    internal sealed class KafkaRequest
    {
        public static readonly KafkaRequest Instance = new KafkaRequest();

        private KafkaRequest()
        {
        }
    }
}
