using BillingInvoicingPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Domain.Entities
{
    public class Invoice:BaseEntity
    {
       
        public string? InvoiceNumber { get; set; } //auto-generated in business 
         public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime? IssueDate { get; set; }=DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public InvoiceStatus Status { get; set; }=InvoiceStatus.Draft;
        public string? Notes { get; set; }



        // ===== Financial Totals (Set by Application Layer) =====
        public decimal SubTotal { get;  set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }



        private readonly List<InvoiceItem> _invoiceItems = new();


        public  ICollection<InvoiceItem> Items => _invoiceItems;

        private readonly List<Payment> _payments = new();

        public ICollection<Payment> Payments => _payments;
        public decimal TotalPaid => Payments.Sum(p => p.PaymentAmount);
        public decimal RemainingBalance => TotalAmount - TotalPaid;

        public int DaysOverdue => Status == InvoiceStatus.Overdue
             ? (DateTime.UtcNow.Date - DueDate.Value.Date).Days
             : 0;
    }
}
