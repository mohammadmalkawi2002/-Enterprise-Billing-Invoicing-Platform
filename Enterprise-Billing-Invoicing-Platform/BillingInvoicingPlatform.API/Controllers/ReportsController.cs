using BillingInvoicingPlatform.Application.Dto.Reports;
using BillingInvoicingPlatform.Application.Service.Abstraction;
using BillingInvoicingPlatform.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    [Authorize(Roles = nameof(Roles.Admin)+","+nameof(Roles.Accountant))]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [Route("outstanding-receivables")]
       
        public async Task<ActionResult<OutstandingReceivablesReportDto>> GetOutstandingReceivables(
            [FromQuery] OutstandingReceivablesQueryDto query)
        {
            var report = await _reportService.GetOutstandingReceivablesReportAsync(query);
            return Ok(report);
        }


        //[HttpGet]
        //[Route("revenue-summary")]
       
        //public async Task<ActionResult<RevenueSummaryReportDto>> GetRevenueSummary(
        //    [FromQuery] RevenueSummaryQueryDto query)
        //{
        //    var report = await _reportService.GetRevenueSummaryReportAsync(query);
        //    return Ok(report);
        //}


    }
}
