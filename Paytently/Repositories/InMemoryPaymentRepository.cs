using Paytently.Models;
using System;
using System.Collections.Generic;

namespace Paytently.Repositories;
/// <summary>
/// This is a dictionary that stores the payment data in memory
/// </summary>
public class InMemoryPaymentRepository : IPaymentRepository {
    private readonly Dictionary<Guid, PaymentResponse> _payments = new();

    public void Save(PaymentResponse response) {
        _payments[response.PaymentId] = response;
    }

    public string GetStatus(Guid paymentId) {
        return _payments.TryGetValue(paymentId, out var response) ? response.Status : "NotFound";
    }
}
