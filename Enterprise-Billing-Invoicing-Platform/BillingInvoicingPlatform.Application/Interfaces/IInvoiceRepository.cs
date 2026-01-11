using BillingInvoicingPlatform.Application.Common.Pagination;
using BillingInvoicingPlatform.Application.Dto.Invoice;
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
        Task<bool> InvoiceExistsAsync(int invoiceId);

        Task SaveChangesAsync();

       // later added:
        //    Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber);
        //Task<List<Invoice>> GetByCustomerIdAsync(int customerId);
        //Task<List<Invoice>> GetOverdueInvoicesAsync();



    }


    
}
