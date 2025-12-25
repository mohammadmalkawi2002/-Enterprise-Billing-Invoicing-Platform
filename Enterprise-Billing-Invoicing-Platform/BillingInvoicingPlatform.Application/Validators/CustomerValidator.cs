using BillingInvoicingPlatform.Application.Dto.Customer;
using BillingInvoicingPlatform.Application.Interfaces;
using BillingInvoicingPlatform.Domain.ValueObjects;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Validators
{
    public class CreateCustomerValidator:AbstractValidator<CreateCustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;
        public CreateCustomerValidator(ICustomerRepository customerRepository) 
        {
            _customerRepository = customerRepository;

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Customer {PropertyName} is required")
                .Length(2, 100)
                .WithMessage("Customer name must be between 2 and 100 characters");


            RuleFor(c => c.Email)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .EmailAddress()
                .WithMessage("Invalid {PropertyName} format")
                .Matches(@"^[^@\s]+@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$")
        .WithMessage("{PropertyName} must have a valid domain (e.g., example.com)");


            //Email Must Be Unique
            RuleFor(c => c.Email)
                .MustAsync(BeUniqueEmail)
                .WithMessage("{PropertyName} {PropertyValue} is already registered");

            RuleFor(c => c.Phone).NotEmpty()
                .WithMessage("{PropertyName} is required")
            .MaximumLength(20)
            .WithMessage("Phone number must not exceed 20 characters");


            //Complex Properties(Like Address)
            /* 1)Address = null:
               * {
            "name": "ABC Company",
            "email": "info@abc.com",
            "phone": "+962791234567",
             "address": null
                }==> valid

            2)partial part from Address exist (error):

            * {
        "address": {
         "country": "",
         "city": "Amman"
            }
            } ==>Not Valid

       */
            RuleFor(c=>c.Address)
               .SetValidator(new AddressValidator())
                .When(x => x.Address != null);

        }



        private async Task<bool> BeUniqueEmail(string email, CancellationToken token)
        {
            return ! await _customerRepository.ExistsByEmailAsync(email);
        }
    }


    public class AddressValidator : AbstractValidator<AddressDto>
    {
       
        public AddressValidator()
        {
            RuleFor(x => x.Country)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(x => x.City)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Street)
                .MaximumLength(150);

            RuleFor(x => x.PostalCode)
                .MaximumLength(5);
        }
    }


    public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerDto> 
    {
        private readonly ICustomerRepository _customerRepository;
        public UpdateCustomerValidator(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;


            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("Customer {PropertyName} is required")
                .GreaterThan(0)
                .WithMessage("Customer Id must be greater than 0");


            RuleFor(c => c.Name)
               .NotEmpty()
               .WithMessage("Customer {PropertyName} is required")
               .Length(2, 100)
               .WithMessage("Customer name must be between 2 and 100 characters");


            RuleFor(c => c.Email)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .EmailAddress()
                .WithMessage("Invalid {PropertyName} format");


            //Email Must Be Unique
            RuleFor(c => c)
                .MustAsync(BeUniqueEmailForUpdate)
                .WithMessage("{PropertyName} {PropertyValue} is already registered");

            RuleFor(c => c.Phone).NotEmpty()
                .WithMessage("{PropertyName} is required")
            .MaximumLength(20)
            .WithMessage("Phone number must not exceed 20 characters");


            RuleFor(c => c.Address)
               .SetValidator(new AddressValidator())
                .When(x => x.Address != null);



        }


        /// <summary>
        /// Checks if the email is unique for updating a customer.
        /// 1. If the email does not exist in the database, it is valid (unique).
        /// 2. If the email exists, but belongs to the same customer being updated, it is still valid.
        /// 3. If the email exists and belongs to a different customer, it is invalid.
        /// </summary>
        private async Task<bool> BeUniqueEmailForUpdate(UpdateCustomerDto dto, CancellationToken token)
        {
            //check if email is used by any customer
            var emailExists =await _customerRepository.ExistsByEmailAsync(dto.Email);

            if(!emailExists)
            {
                //Email not used by any customer(unique=> valid)
                return true;
            }

            //Email exists, check if it belongs to the same customer being updated
            var existingCustomer = await _customerRepository.GetByIdAsync(dto.Id);

            if(existingCustomer.Email == dto.Email)
            {
                //Email belongs to the same customer being updated(unique=> valid)
                return true;
            }

            return false;



        }
    }


}
