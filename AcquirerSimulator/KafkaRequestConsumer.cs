using Confluent.Kafka;
using Paytently.Kafka;
using Paytently.Models;
using System.Text.Json;

namespace AcquirerSimulator;
/// <summary>
/// This is the Kafka consumer that will be running in a worker, so it is listening from the payment-requests topic, 
/// then it generates a random status for the payment and finally it produces a new message to Kafka into payment-responses topic
/// </summary>
public class KafkaRequestConsumer : BackgroundService
{
    // Kafka setup data
    private readonly string _broker = "kafka:9092";
    private readonly string _requestTopic = "payment-requests";
    private readonly string _responseTopic = "payment-responses";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _broker,
            GroupId = "paytently-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _broker
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();

        // It subscribes to the requests topic, where the payment requests are being sent
        consumer.Subscribe(_requestTopic);
        Console.WriteLine("Acquirer Simulator is listening for payment requests...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // When there is a request, it deserialize the message
                var consumeResult = consumer.Consume(stoppingToken);
                var paymentRequest = JsonSerializer.Deserialize<PaymentRequest>(consumeResult.Message.Value);

                Console.WriteLine($"Received payment request: {paymentRequest?.PaymentId}");

                // It generates a random response for the payment
                var success = new Random().Next(0, 2) == 1;

                // It creates the response message, with the card number masked
                var response = new PaymentResponse
                {
                    PaymentId = paymentRequest?.PaymentId ?? Guid.NewGuid(),
                    Amount = paymentRequest?.Amount ?? 0,
                    CardNumber = MaskCardNumber(paymentRequest?.CardNumber ?? string.Empty),
                    Currency = paymentRequest?.Currency ?? string.Empty,
                    Status = success ? "Success" : "Failure"
                };
                var responseJson = JsonSerializer.Serialize(response);

                // It sends a new message into payment-responses topic, this message will be proccesed in the api
                await producer.ProduceAsync(_responseTopic, new Message<Null, string> { Value = responseJson });
                producer.Flush(TimeSpan.FromSeconds(1));
                Console.WriteLine($"Sent payment response: {response.Status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing payment request: {ex.Message}");
            }
        }
    }

    // This is the method to mask the card number
    public string MaskCardNumber(string cardNumber)
    {
        if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 4)
            return "****";

        return new string('*', cardNumber.Length - 4) + cardNumber[^4..];
    }
}
