using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Domain.Entities
{
    public class InvoiceItem:BaseEntity
    {
        public int InvoiceId { get; set; }
        public Invoice Invoice { get;  set; } = default!;

        public string Description { get;  set; } = default!;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }

        public decimal LineSubTotal => Quantity * UnitPrice;

        public decimal LineTax => LineSubTotal * TaxRate / 100;

        public decimal LineTotal => LineSubTotal + LineTax;

    }
}
