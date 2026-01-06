using AutoMapper;
using BillingInvoicingPlatform.Application.Common.Pagination;
using BillingInvoicingPlatform.Application.Dto.Customer;
using BillingInvoicingPlatform.Application.Exceptions;
using BillingInvoicingPlatform.Application.Interfaces;
using BillingInvoicingPlatform.Application.Service.Abstraction;
using BillingInvoicingPlatform.Domain.Entities;
using BillingInvoicingPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Service
{
    public class CustomerService: ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        public CustomerService(ICustomerRepository customerRepository,IMapper mapper) 
        { 
            _customerRepository = customerRepository;
            _mapper = mapper;
        }


        public async Task<PagedResult<CustomerDto>> GetAllCustomer(CustomerQueryDto queryDto)
        {
            //queryDto.PageNumber= queryDto.PageNumber <= 0 ? 1 : queryDto.PageNumber;
            //queryDto.PageSize= queryDto.PageSize <= 0 ? 10 : queryDto.PageSize;


            var result = await _customerRepository.GetPagedAsync(queryDto);


            return new PagedResult<CustomerDto>
            {
                Items = _mapper.Map<List<CustomerDto>>(result.Items),
                TotalCount = result.TotalCount,
                PageSize = result.PageSize,
                PageNumber = result.PageNumber
            };
        }

        public async Task<CustomerDto> GetCustomerById(int id) 
        { 
            var customer= await _customerRepository.GetByIdAsync(id);
            if (customer is null)
                throw new NotFoundException();

          return  _mapper.Map<CustomerDto>(customer);
        
        }

         public  async Task<CustomerDto> CreateCustomer(CreateCustomerDto CustomerDto) 
        {
            //check email is unique
            var emailExist = await _customerRepository.ExistsByEmailAsync(CustomerDto.Email);

            if (emailExist)
                throw new BusinessException($"Email `{CustomerDto.Email}` is already registered");

            //1-Mapping From Dto to Entity:
            var customer=  _mapper.Map<Customer>(CustomerDto);

            //2- add createdAt +Status is Active
                 customer.CreatedAt=DateTime.UtcNow;
                customer.Status=CustomerStatus.Active;

            //3-Add to the Db by call Repo:
             var created= await _customerRepository.AddAsync(customer);

            return _mapper.Map<CustomerDto>(created);

        
        }

        
        public async Task UpdateCustomer( UpdateCustomerDto updateCustomerDto) 
        {

            //Load the existing customer
            var customer = await _customerRepository.GetByIdAsync(updateCustomerDto.Id);
            if (customer is null)
                throw new NotFoundException($" customer with Id {updateCustomerDto.Id} NotFound");


            //// 2. Business Rule: Email must be unique
             await EnsureEmailIsUniqueForUpdate(updateCustomerDto,customer);


            customer.UpdatedAt=DateTime.UtcNow;
            _mapper.Map(updateCustomerDto, customer);

            await _customerRepository.UpdateAsync(customer);
        }

        public async Task DeleteCustomerAsync(int customerId) 
        {
            //1- Get the customer
            var customer =await _customerRepository.GetByIdAsync(customerId);

            //2-check if the customer Exist
            if (customer is null)
                throw new NotFoundException($" customer with Id {customerId} NotFound");

            //3- check if Customer has Existing Invoices(prevent delete):

            var hasInvoice= await _customerRepository.HasInvoicesAsync(customerId);

            if (hasInvoice)
                throw new BusinessException($"Cannot delete customer `{customer.Name}`. Customer has {customer.Invoices.Count} existing invoices. Consider marking as inactive instead");

            //4- call softDeleteAsync Repo
            await _customerRepository.SoftDeleteAsync(customer);
        }

        private async Task EnsureEmailIsUniqueForUpdate(UpdateCustomerDto dto, Customer customer) 
        {
            // Check if email is used by another customer(Not Unique)
            var emailExists= await _customerRepository.ExistsByEmailAsync(dto.Email);

            if(!emailExists)
                return;//email is unique(valid)
            //email is exist , check if the email is belong to the same customer

            if(dto.Email==customer.Email)
                return;//valid

            // Used by another customer → NOT OK
            throw new BusinessException(
                $"Email '{dto.Email}' is already registered by another customer.");

        }

    }
}
