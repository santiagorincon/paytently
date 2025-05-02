using Paytently.Models;

namespace Paytently.Kafka;
public interface IKafkaAwaiterService
{
    public Task<PaymentResponse?> WaitForResponseAsync(Guid paymentId, int timeout);
}
