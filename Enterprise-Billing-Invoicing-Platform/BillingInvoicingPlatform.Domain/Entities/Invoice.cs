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
        public decimal TotalPaid =>
    Math.Round(Payments.Sum(p => p.PaymentAmount), 2);
        public decimal RemainingBalance =>
        Math.Max(0, Math.Round(TotalAmount - TotalPaid, 2));

        public int DaysOverdue
        {
            get
            {
                

                if (Status == InvoiceStatus.Draft ||
                    Status == InvoiceStatus.Paid ||
                    Status == InvoiceStatus.Cancelled)
                    return 0;

                var days = (DateTime.UtcNow.Date - DueDate.Value.Date).Days;
                return days > 0 ? days : 0;
            }
        }
    }
}
