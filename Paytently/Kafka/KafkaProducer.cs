using Confluent.Kafka;
using Paytently.Models;
using System.Text.Json;

namespace Paytently.Kafka;
/// <summary>
/// This is the kafka producer, used to send the payment request to the payment-request topic
/// </summary>
public class KafkaProducer : IKafkaProducer
{
    private readonly KafkaSettings _kafkaSettings;

    // It gets the kafka settings from appsettings.json, using dependency injection
    public KafkaProducer(KafkaSettings kafkaSettings)
    {
        _kafkaSettings = kafkaSettings;
    }
    public async Task SendPaymentRequest(PaymentRequest paymentRequest)
    {
        var config = new ProducerConfig { BootstrapServers = _kafkaSettings.Broker };

        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            var paymentRequestJson = JsonSerializer.Serialize(paymentRequest);

            try
            {
                // It sends the payment request to the 'payment-requests' topic
                await producer.ProduceAsync(_kafkaSettings.RequestTopic, new Message<Null, string> { Value = paymentRequestJson });
                producer.Flush(TimeSpan.FromSeconds(_kafkaSettings.ResponseTimeoutSeconds));
                Console.WriteLine("Payment request sent.");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Error producing message to Kafka: {e.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }
    }
}
