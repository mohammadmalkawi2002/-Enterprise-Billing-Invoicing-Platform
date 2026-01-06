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
        Task<Invoice> AddAsync(Invoice invoice);
       
        Task UpdateAsync(Invoice invoice);
        Task SoftDeleteAsync(Invoice invoice);
        Task<string?> GetLastInvoiceNumber();
        Task<InvoiceDto?> GetInvoiceDetailsAsync(int invoiceId);
        Task<Invoice?> GetByIdAsync(int id);
       


        Task<bool> InvoiceExistsAsync(int invoiceId);

       // later added:
        //    Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber);
        //Task<List<Invoice>> GetByCustomerIdAsync(int customerId);
        //Task<List<Invoice>> GetOverdueInvoicesAsync();



    }


    
}
