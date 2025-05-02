namespace Paytently.Kafka;
public class KafkaSettings
{
    public string Broker { get; set; }
    public string RequestTopic { get; set; }
    public string ResponseTopic { get; set; }
    public int ResponseTimeoutSeconds { get; set; }
}
