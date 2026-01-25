using BillingInvoicingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(int id);
        Task<List<Payment>> GetByInvoiceIdAsync(int invoiceId);
        Task<Payment> AddAsync(Payment payment);
        Task  DeleteAsync(Payment payment);



    }
}
