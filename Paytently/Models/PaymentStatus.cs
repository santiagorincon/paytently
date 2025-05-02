using System;

namespace Paytently.Models;
public class PaymentStatus {
    public Guid PaymentId { get; set; }
    public string Status { get; set; }
}
