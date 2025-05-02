using System;
using Paytently.Kafka;
using Paytently.Models;
using Paytently.Repositories;

namespace Paytently.Services;
public class PaymentService : IPaymentService {
    private readonly IPaymentRepository _repository;
    private readonly IKafkaProducer _kafkaProducer;
    private readonly IKafkaAwaiterService _kafkaAwaiterService;
    private readonly KafkaSettings _kafkaSettings;

    public PaymentService(
        IPaymentRepository repository, 
        IKafkaProducer kafkaProducer,
        IKafkaAwaiterService kafkaAwaiterService,
        KafkaSettings kafkaSettings) {
        _repository = repository;
        _kafkaProducer = kafkaProducer;
        _kafkaAwaiterService = kafkaAwaiterService;
        _kafkaSettings = kafkaSettings;
    }

    /// <summary>
    /// This is the method to send the payment request to Kafka, wait for the acquirer to get that request and send a response, 
    /// and finally read the response with the status
    /// </summary>
    /// <param name="request">Payment request</param>
    /// <returns>Payment data (including status)</returns>
    public async Task<PaymentResponse?> ProcessPayment(PaymentRequest request) {
        // It creates the new payment id
        request.PaymentId = Guid.NewGuid();

        // It sends the payment request to kafka
        await _kafkaProducer.SendPaymentRequest(request);

        // It waits for the payment response
        var response = await _kafkaAwaiterService.WaitForResponseAsync(request.PaymentId, _kafkaSettings.ResponseTimeoutSeconds);

        if(response != null)
        {
            // It stores the payment data
            _repository.Save(response);
        }
        return response;
    }

    /// <summary>
    /// This is to get the payment status
    /// </summary>
    /// <param name="paymentId">Payment id to get the status</param>
    /// <returns>Payment status (Success, Failure, NotFound)</returns>
    public PaymentStatus GetPaymentStatus(Guid paymentId) {
        var status = _repository.GetStatus(paymentId);
        return new PaymentStatus {
            PaymentId = paymentId,
            Status = status
        };
    }
}
