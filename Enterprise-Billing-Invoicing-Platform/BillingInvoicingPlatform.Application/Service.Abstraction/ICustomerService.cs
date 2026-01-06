using BillingInvoicingPlatform.Application.Common.Pagination;
using BillingInvoicingPlatform.Application.Dto.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Service.Abstraction
{
    public interface ICustomerService
    {
        Task<PagedResult<CustomerDto>> GetAllCustomer(CustomerQueryDto queryDto);
        Task<CustomerDto> GetCustomerById(int id);
        Task<CustomerDto> CreateCustomer(CreateCustomerDto CustomerDto);
        Task UpdateCustomer(UpdateCustomerDto updateCustomerDto);
        Task DeleteCustomerAsync(int customerId);
    }
}
