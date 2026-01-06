using BillingInvoicingPlatform.Application.Dto.Invoice;
using BillingInvoicingPlatform.Application.Dto.Payment;
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
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _dbContext;
   
        public InvoiceRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }



        public async Task<InvoiceDto?> GetInvoiceDetailsAsync(int invoiceId)
        {



            return await _dbContext.Invoices.AsNoTracking()
                .Where(i => i.Id == invoiceId)
                .Select(i => new InvoiceDto
                {

                    InvoiceNumber = i.InvoiceNumber,
                    CustomerId = i.CustomerId,
                    CustomerName = i.Customer.Name,
                    IssueDate = i.IssueDate ?? default(DateTime),
                    DueDate = i.DueDate,
                    CreatedAt = i.CreatedAt ?? default(DateTime),
                    Status = i.Status.ToString(),
                    SubTotal = i.SubTotal,
                    TaxAmount = i.TaxAmount,
                    TotalAmount = i.TotalAmount,
                    TotalPaid = i.TotalPaid,
                    RemainingAmount = i.RemainingBalance,
                    DaysOverdue = i.DaysOverdue,

                    Items = i.Items.Select(item => new InvoiceItemDto
                    {
                        Id = item.Id,
                        Description = item.Description,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TaxRate = item.TaxRate,
                        LineTotal = item.LineTotal
                    }).ToList(),
                    Payments = i.Payments.Select(payment => new PaymentDto
                    {
                        Amount = payment.PaymentAmount,
                        PaymentDate = payment.PaymentDate,
                        PaymentMethod = payment.PaymentMethod.ToString(),
                        ReferenceNumber = payment.ReferenceNumber,
                        Notes = payment.Note,
                    }).ToList()

                })
                .FirstOrDefaultAsync();
            //TODO: Try use Automapper in Repository later 
        }

     


        public async Task<Invoice?> GetByIdAsync(int id) 
        {
           
            return await _dbContext.Invoices    
                    .Include(i => i.Customer)
                    .Include(i => i.Items)
                    .Include(i => i.Payments)
                    .FirstOrDefaultAsync(i=> i.Id == id);
                 

        }

     
        public async Task<Invoice> AddAsync(Invoice invoice)
        {
           await _dbContext.Invoices.AddAsync(invoice);
           await  _dbContext.SaveChangesAsync();
              return invoice;
        }

        public async Task UpdateAsync(Invoice invoice)
        { 
            _dbContext.Invoices.Update(invoice);

          await  _dbContext.SaveChangesAsync();
        }



        public async Task SoftDeleteAsync(Invoice invoice)
        {
            invoice.IsDeleted = true;
            invoice.DeletedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }
        public async Task<bool> InvoiceExistsAsync(int invoiceId)
        {
            return await _dbContext.Invoices
                .AsNoTracking()
                .AnyAsync(i => i.Id == invoiceId);
        }



        /// <summary>
        /// Get the last invoice number for the current year from the database ex
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the invoice number of the latest
        /// invoice for the current year, or null if no invoices exist for the current year.</returns>
        public async Task<string?> GetLastInvoiceNumber()
        {

            return await _dbContext.Invoices
                .Where(i => i.CreatedAt.Value.Year == DateTime.UtcNow.Year)
                .OrderByDescending(i => i.Id)
                .Select(i => i.InvoiceNumber)
                .FirstOrDefaultAsync();


        }


       
    }
}
