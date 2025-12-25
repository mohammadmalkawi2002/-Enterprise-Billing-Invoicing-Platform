using BillingInvoicingPlatform.Application.Dto.Customer;
using BillingInvoicingPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Dto
{
    public class InvoiceDto
    {
        public int Id { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; }

        public DateTime DueDate { get; set; }

        public InvoiceStatus Status { get; set; }

        
        public decimal TotalAmount { get; set; }

        public decimal RemainingBalance { get; set; }
    }



    public class InvoiceDetailDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public CustomerDto Customer { get; set; } = new CustomerDto();
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public InvoiceStatus Status { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal RemainingBalance { get; set; }
      //  public IReadOnlyList<InvoiceItemDto> Items { get; set; } = new List<InvoiceItemDto>();
        //public IReadOnlyList<PaymentDto> Payments { get; set; } = new List<PaymentDto>();
    }

    public class CreateInvoiceDto
    {
    }

    public class UpdateInvoiceDto
    {
    }
}
