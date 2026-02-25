using BillingInvoicingPlatform.Application.Dto.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Service.Abstraction
{
    /// <summary>
    /// Service for generating outstanding receivables reports
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Generate an outstanding receivables report showing all unpaid/partially paid invoices
        /// with comprehensive filtering and summary statistics
        /// </summary>
        /// <param name="query">Filter parameters for the report</param>
        /// <returns>Complete report with line items and summary statistics</returns>
        Task<OutstandingReceivablesReportDto> GetOutstandingReceivablesReportAsync(OutstandingReceivablesQueryDto query);
        Task<RevenueSummaryReportDto> GetRevenueSummaryReportAsync(RevenueSummaryQueryDto query);
    }
}
