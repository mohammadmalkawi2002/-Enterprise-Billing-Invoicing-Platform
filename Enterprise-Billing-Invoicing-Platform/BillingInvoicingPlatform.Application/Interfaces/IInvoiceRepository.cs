using BillingInvoicingPlatform.Application.Common.Pagination;
using BillingInvoicingPlatform.Application.Dto.Invoice;
using BillingInvoicingPlatform.Application.Dto.Reports;
using BillingInvoicingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<PagedResult<InvoiceDtoPagination>> GetPagedAsync(InvoiceQueryDto query);
        Task<Invoice> AddAsync(Invoice invoice);
       
        Task UpdateAsync(Invoice invoice);
        Task SoftDeleteAsync(Invoice invoice);
        Task<string?> GetLastInvoiceNumber();
        Task<InvoiceDto?> GetInvoiceDetailsAsync(int invoiceId);
        Task<Invoice?> GetByIdAsync(int id);
        Task<List<Invoice>> GetOverdueInvoicesAsync();
        Task<List<OutstandingReceivableItemDto>> GetOutstandingReceivablesAsync(OutstandingReceivablesQueryDto query);
        Task <List<InvoiceDto>> GetRevenueDataAsync(RevenueSummaryQueryDto query);

        /// <summary>
        /// Returns invoices (including Customer navigation) that are due in `days` days
        /// and are in statuses that require reminders (e.g., Sent, PartiallyPaid).
        /// </summary>
  

        Task SaveChangesAsync();

       



    }


    
}
