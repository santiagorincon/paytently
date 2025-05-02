using Paytently.Models;

namespace Paytently.Kafka;
public interface IKafkaProducer
{
    Task SendPaymentRequest(PaymentRequest paymentRequest);
}
