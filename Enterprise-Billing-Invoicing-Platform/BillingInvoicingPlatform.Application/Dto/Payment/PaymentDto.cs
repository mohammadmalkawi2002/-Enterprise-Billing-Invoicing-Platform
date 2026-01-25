using BillingInvoicingPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Dto.Payment
{
    public class CreatePaymentDto
    {
        public int InvoiceId { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string? ReferenceNumber { get; set; }
        public string? Note { get; set; }
    }


    public class PaymentDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? PaymentMethod { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Note { get; set; }
    }
}
