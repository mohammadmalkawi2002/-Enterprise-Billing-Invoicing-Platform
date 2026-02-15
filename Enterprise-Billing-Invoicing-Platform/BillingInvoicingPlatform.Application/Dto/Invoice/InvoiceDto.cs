using BillingInvoicingPlatform.Application.Dto.Customer;
using BillingInvoicingPlatform.Application.Dto.Payment;
using BillingInvoicingPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Dto.Invoice
{
    /// <summary>
    /// For creating a new invoice (Draft)
    /// </summary>
    public class CreateInvoiceDto
    {
        public int CustomerId { get; set; }
       
        public DateTime? IssueDate { get; set; } 

        public DateTime? DueDate { get; set; }

        public string? Notes { get; set; }

        public List<CreateInvoiceItemDto> Items { get; set; } = new();
    }


    /// <summary>
    /// For updating an existing invoice
    /// </summary>
    public class UpdateInvoiceDto
    {

        
        public DateTime? IssueDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Notes { get; set; }
        public List<UpdateInvoiceItemDto> Items { get; set; } = new();
    }



    /// <summary>
    /// (For adding items to an invoice)
    /// Represents the data required to create a new invoice item.
    /// </summary>
    public class CreateInvoiceItemDto
    {
        public string Description { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal? TaxRate { get; set; }
    }


    public class UpdateInvoiceItemDto
    {
        public int? Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? TaxRate { get; set; }
    }

    /// <summary>
    /// Represents a data transfer object containing invoice details, including customer information, invoice status,
    /// dates, amounts, and line items.
    /// </summary>
    /// <remarks>This class is typically used to transfer invoice data between application layers or services.
    /// It includes properties for tracking payment status, invoice amounts, and associated items. All monetary values
    /// are represented in the currency relevant to the invoice. The class does not enforce business rules or
    /// validation; consumers are responsible for ensuring property values are valid as required by their use
    /// case.</remarks>
    public class InvoiceDto
    {
       
        public string? InvoiceNumber { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; }
        public DateTime? DueDate { get; set; }

        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal RemainingAmount { get; set; }
        public int DaysOverdue { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<InvoiceItemDto> Items { get; set; } = new();
        public List<PaymentDto> Payments { get; set; } = new();
    }



    public class InvoiceDtoPagination
    {
        public string? InvoiceNumber { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; }
        public DateTime? DueDate { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal RemainingAmount { get; set; }

        public List<InvoiceItemDto> Items { get; set; } = new();
        public List<PaymentDto> Payments { get; set; } = new();
    }

    public class InvoiceItemDto
    {
        public int Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }

        //public decimal LineSubTotal { get; set; }
        //public decimal LineTax { get; set; }
        public decimal LineTotal { get; set; }
    }



 public class ChangeStatusRequest
    {
        public InvoiceStatus Status { get; set; }
    }



        








}
