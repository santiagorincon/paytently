using System;
using Paytently.Models;

namespace Paytently.Services;
public interface IPaymentService {
    Task<PaymentResponse?> ProcessPayment(PaymentRequest request);
    PaymentStatus GetPaymentStatus(Guid paymentId);
}
