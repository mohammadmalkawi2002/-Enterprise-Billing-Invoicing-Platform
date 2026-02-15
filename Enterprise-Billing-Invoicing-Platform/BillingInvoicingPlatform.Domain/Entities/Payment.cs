using BillingInvoicingPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Domain.Entities
{
    public class Payment:BaseEntity
    {
      
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate {  get; set; }
        public PaymentMethod PaymentMethod { get; set; }=PaymentMethod.Cash;

        public string? ReferenceNumber {  get; set; }
        public string? Note { get; set; }


    }
}
