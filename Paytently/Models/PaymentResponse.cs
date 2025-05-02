using System;

namespace Paytently.Models;
public class PaymentResponse {
    public Guid PaymentId { get; set; }
    public string CardNumber { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Status { get; set; }
}
