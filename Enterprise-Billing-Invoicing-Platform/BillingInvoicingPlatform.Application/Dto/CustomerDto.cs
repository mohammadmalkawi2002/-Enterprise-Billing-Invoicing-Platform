using BillingInvoicingPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Dto
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public CustomerStatus Status { get; set; }
        //any additional info about Invoice: (Question For Mohammad)
        public int InvoiceCount { get; set; }
    }


    public class CreateCustomerDto 
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty!;
        public string Phone { get; set; } = string.Empty!;
      public AddressDto Address { get; set; }

    }

    public class UpdateCustomerDto :CreateCustomerDto
    { 
        public int Id { get; set; }
    
    }


    public class AddressDto 
    {
        public string? Country { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? Street { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
    }
}
