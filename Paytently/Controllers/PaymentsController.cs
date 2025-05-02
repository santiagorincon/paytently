using Microsoft.AspNetCore.Mvc;
using Paytently.Models;
using Paytently.Services;
using System;

namespace Paytently.Controllers {
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase {
        private readonly IPaymentService _service;

        // Dependency injection to get the payment service
        public PaymentsController(IPaymentService service) {
            _service = service;
        }

        // This is the endpoint to send a new payment request, it will return the status of the payment
        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request) {
            var response = await _service.ProcessPayment(request);
            return Ok(response);
        }

        // This is the endpoint to get the status of a specific payment, it will return NotFound when the payment doesn't exist
        [HttpGet("{id}")]
        public IActionResult GetStatus(Guid id) {
            var status = _service.GetPaymentStatus(id);
            return Ok(status);
        }
    }
}
