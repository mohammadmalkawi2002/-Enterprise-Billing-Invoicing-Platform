using BillingInvoicingPlatform.Application.Dto.Payment;
using BillingInvoicingPlatform.Application.Service.Abstraction;
using BillingInvoicingPlatform.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BillingInvoicingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        [HttpGet]
        [Route("{paymentId:int}")]
        [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Accountant) + "," + nameof(Roles.User))]
        public async Task<ActionResult<PaymentDto>> GetById(int paymentId)
        {
         var payment=await _paymentService.GetPaymentById(paymentId);
            return Ok(payment);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Accountant))]

        public async Task<ActionResult<PaymentDto>> Create([FromBody] CreatePaymentDto dto) 
        { 
                
            var payment= await _paymentService.RecordPayment(dto);
            return CreatedAtAction(nameof(Create), new { id = payment.Id }, payment);


        }

        [HttpDelete]
        [Route("{paymentId:int}")]
        [Authorize(Roles = nameof(Roles.Admin))]

        public async Task<IActionResult> Delete(int paymentId) 
        {
            await _paymentService.DeletePaymentAsync(paymentId);
            return NoContent();
        }
    }
}
