using BillingInvoicingPlatform.Domain.Enums;
using BillingInvoicingPlatform.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Domain.Entities
{
    public class Customer:BaseEntity
    {
          public string Name { get; set; }=string.Empty;
        public string Email { get; set; } = string.Empty!;
        public string Phone { get; set; } = string.Empty!;
        
        public Address Address { get; set; } = new();

        public CustomerStatus Status { get; set; } = CustomerStatus.Active;
     public  ICollection<Invoice> Invoices { get; set; }=new List<Invoice>();

    }
}
