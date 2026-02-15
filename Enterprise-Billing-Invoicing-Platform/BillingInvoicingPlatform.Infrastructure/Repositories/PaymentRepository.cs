using BillingInvoicingPlatform.Application.Interfaces;
using BillingInvoicingPlatform.Domain.Entities;
using BillingInvoicingPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Infrastructure.Repositories
{
    public class PaymentRepository:IPaymentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PaymentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Payment> AddAsync(Payment payment)
        {
           await  _dbContext.AddAsync(payment);
            await _dbContext.SaveChangesAsync();
            return payment;
        }

        public async Task DeleteAsync(Payment payment)
        {
           payment.IsDeleted = true;
            payment.DeletedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
           return await _dbContext.Payments
                    .Include(p=>p.Invoice)
                    .ThenInclude(i=>i.Customer)
                    .FirstOrDefaultAsync(p=>p.Id == id);
        }

        public Task<List<Payment>> GetByInvoiceIdAsync(int invoiceId)
        {
           return _dbContext.Payments
                    .Where(p=>p.InvoiceId == invoiceId)
                    .Include(p=>p.Invoice)
                    .ToListAsync();
        }
    }
}
