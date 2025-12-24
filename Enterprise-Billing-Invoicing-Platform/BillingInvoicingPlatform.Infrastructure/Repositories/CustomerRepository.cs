using BillingInvoicingPlatform.Application.Interfaces;
using BillingInvoicingPlatform.Domain.Entities;
using BillingInvoicingPlatform.Domain.Enums;
using BillingInvoicingPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext) 
        { 
            _dbContext = dbContext;
        }
        public async Task<Customer> AddAsync(Customer customer)
        {
           await _dbContext.AddAsync(customer);

            await _dbContext.SaveChangesAsync();
            return customer;

        }

      

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            var exists = await _dbContext.Customers.AnyAsync(c => c.Email == email);
           return exists  ;
        }

        public async Task<List<Customer>> GetActiveAsync()
        {
            return await _dbContext.Customers.AsNoTracking().Where(c=>c.Status== CustomerStatus.Active).ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
             return await _dbContext.Customers
                .Include(c=>c.Invoices)
                .AsSplitQuery()
                .FirstOrDefaultAsync(c=>c.Id == id);
                
        }

        public async Task<bool> HasInvoicesAsync(int customerId)
        {

            return await _dbContext.Invoices.AnyAsync(i=>i.CustomerId == customerId && !i.IsDeleted);
        }

        public async Task SoftDeleteAsync(Customer customer)
        {
          customer.Status=CustomerStatus.Deleted;

            customer.DeletedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            _dbContext.Customers.Update(customer);
           await _dbContext.SaveChangesAsync();
        }
    }
}
