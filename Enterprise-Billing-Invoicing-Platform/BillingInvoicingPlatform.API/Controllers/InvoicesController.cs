using BillingInvoicingPlatform.Application.Service.Abstraction;
using BillingInvoicingPlatform.Application.Exceptions;
using BillingInvoicingPlatform.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BillingInvoicingPlatform.Application.Dto.Invoice;
using System.Text.Json;

namespace BillingInvoicingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
       

        public InvoicesController(IInvoiceService invoiceService)
        { 
            _invoiceService = invoiceService;
           
        }



        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<InvoiceDtoPagination>>> GetAllInvoices([FromQuery] InvoiceQueryDto queryDto)
        {
            var pagedResult = await _invoiceService.GetAllInvoicesAsync(queryDto);
            var metaData = new 
            {
                pagedResult.TotalPages,
                pagedResult.TotalCount,
                pagedResult.PageSize,
                pagedResult.PageNumber,
                pagedResult.HasPrevious,
                pagedResult.HasNext

            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

            return Ok(pagedResult.Items);
        }


        [HttpPost]
        public async Task<ActionResult<InvoiceDto>> CreateInvoice([FromBody] CreateInvoiceDto dto)
        {

            var invoice = await _invoiceService.CreateAsync(dto);
            return CreatedAtAction(nameof(CreateInvoice), new { }, invoice);

        }



        [HttpPut]
        [Route("{invoiceId}")]
        public async Task<IActionResult> UpdateInvoice(int invoiceId, [FromBody] UpdateInvoiceDto dto)
        {
            await _invoiceService.UpdateAsync(invoiceId, dto);
            return NoContent();
        }


     

        [HttpPatch("{invoiceId}/status")]
        public async Task<ActionResult<InvoiceDto>> ChangeStatus(
           int invoiceId,
           [FromBody] ChangeStatusRequest request)
        {
            var invoice = await _invoiceService.ChangeInvoiceStatusAsync(invoiceId, request.Status);
            return Ok(invoice);


        }

        [HttpGet]
        [Route("{invoiceId}/WithDetails")]
        public async Task<ActionResult<InvoiceDto>> GetInvoiceWithDetails(int invoiceId)
        {
            var invoice = await _invoiceService.GetInvoiceWithDetailsAsync(invoiceId);
            if (invoice is null)
                return NotFound();
            return Ok(invoice);
        }

       

        [HttpDelete]
        [Route("{invoiceId}")]
        public async Task<IActionResult> Delete(int invoiceId) 
        {
            await _invoiceService.DeleteInvoice(invoiceId);
            return NoContent();
        }
    }
}
