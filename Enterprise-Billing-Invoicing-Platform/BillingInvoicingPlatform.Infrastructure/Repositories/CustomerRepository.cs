using BillingInvoicingPlatform.Application.Common.Pagination;
using BillingInvoicingPlatform.Application.Dto.Customer;
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

        public async Task<PagedResult<Customer>> GetPagedAsync(CustomerQueryDto query)
        {
            var customers = _dbContext.Customers
                                 .AsNoTracking()
                                 .Where(c => c.Status != CustomerStatus.Deleted);

            // Searching by Name or Email

            if (!string.IsNullOrEmpty(query.SearchBy)) 
            {
                var search=query.SearchBy.Trim().ToLower();
                customers = customers.Where(c => 
                               c.Name !=null &&  c.Name.ToLower().Contains(search)
                              || c.Email !=null &&  c.Email.ToLower().Contains(search));
            }

            // Filtering by Status or 
            if (!string.IsNullOrEmpty(query.Status) &&
                Enum.TryParse<CustomerStatus>(query.Status, true, out var status))
            {
                customers = customers.Where(c => c.Status == status);
            }

            // Sorting(default by Id):

            customers =query.SortBy?.ToLower() switch
            {
                "name" => query.SortDirection?.ToLower() == "desc" 
                ? customers.OrderByDescending(c => c.Name) 
                : customers.OrderBy(c => c.Name),

                "email" => query.SortDirection?.ToLower() == "desc" 
                ? customers.OrderByDescending(c => c.Email) 
                : customers.OrderBy(c => c.Email),

                "createdAt" => query.SortDirection?.ToLower() == "desc" 
                ? customers.OrderByDescending(c => c.CreatedAt) 
                : customers.OrderBy(c => c.CreatedAt),

                _ => customers.OrderBy(c => c.Id),
            };

            var totalCount = await customers.CountAsync();

            // Pagination:

            var items = await customers
                         .Skip((query.PageNumber - 1) * query.PageSize)
                          .Take(query.PageSize)
                             .ToListAsync();


            return new PagedResult<Customer> 
            { 
                 Items = items,
                 TotalCount = totalCount,
                 PageSize = query.PageSize,
                 PageNumber = query.PageNumber

            };

        }


        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _dbContext.Customers
               .Include(c => c.Invoices)
               .AsSplitQuery()
               .FirstOrDefaultAsync(c => c.Id == id);

        }



        public async Task<Customer> AddAsync(Customer customer)
        {
           await _dbContext.AddAsync(customer);

            await _dbContext.SaveChangesAsync();
            return customer;

        }


        public async Task UpdateAsync(Customer customer)
        {
            _dbContext.Customers.Update(customer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(Customer customer)
        {
            customer.Status = CustomerStatus.Deleted;

            customer.DeletedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
        }


        public async Task<bool> ExistsByEmailAsync(string email)
        {
            var exists = await _dbContext.Customers.AnyAsync(c => c.Email == email );
           return exists  ;
        }

        public async Task<List<Customer>> GetActiveAsync()
        {
            return await _dbContext.Customers.AsNoTracking().Where(c=>c.Status== CustomerStatus.Active).ToListAsync();
        }

        public Task<Customer?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

       public async Task<bool> IsCustomerActiveAsync(int customerId) 
        { 
            return await _dbContext.Customers
                .AnyAsync(c => c.Id == customerId && c.Status == CustomerStatus.Active);
        }



        public async Task<bool> HasInvoicesAsync(int customerId)
        {

            return await _dbContext.Invoices.AnyAsync(i=>i.CustomerId == customerId && !i.IsDeleted);
        }

        public Task<bool> ExistsAsync(int customerId)
        {
            return  _dbContext.Customers.AnyAsync(c => c.Id == customerId );

        }
    }
}
