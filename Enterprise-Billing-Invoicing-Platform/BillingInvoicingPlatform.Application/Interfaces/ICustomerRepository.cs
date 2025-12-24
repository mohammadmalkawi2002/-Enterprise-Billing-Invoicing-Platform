using BillingInvoicingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(int id);
         Task<Customer> AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);


        // ===== Business-support Queries =====

        /// <summary>
        /// Checks if a customer with the given email already exists.
        /// Used for uniqueness validation in Application layer.
        /// </summary>
        Task<bool> ExistsByEmailAsync(string email);

        /// <summary>
        /// Checks if customer has any invoices.
        /// Used to prevent deletion.
        /// </summary>
        Task<bool> HasInvoicesAsync(int customerId);
        Task SoftDeleteAsync(Customer customer);

        /// <summary>
        /// Returns only active customers (Status = Active)
        /// </summary>
        Task<List<Customer>> GetActiveAsync();


    }
}
