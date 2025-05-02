namespace Paytently.Models;
public class PaymentRequest {
    public Guid PaymentId { get; set; } = Guid.NewGuid();
    public string CardNumber { get; set; }
    public string ExpiryMonth { get; set; }
    public string ExpiryYear { get; set; }
    public string CVV { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
}
