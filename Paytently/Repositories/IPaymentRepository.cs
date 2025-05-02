using Paytently.Models;
using System;

namespace Paytently.Repositories;
public interface IPaymentRepository {
    void Save(PaymentResponse response);
    string GetStatus(Guid paymentId);
}
