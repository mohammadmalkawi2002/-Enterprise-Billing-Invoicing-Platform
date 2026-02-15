using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Domain.ValueObjects
{
    public class Address
    {
        public string Country { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string?Street { get; set; } = string.Empty;
        public string? PostalCode { get; set; } 
    }
}
