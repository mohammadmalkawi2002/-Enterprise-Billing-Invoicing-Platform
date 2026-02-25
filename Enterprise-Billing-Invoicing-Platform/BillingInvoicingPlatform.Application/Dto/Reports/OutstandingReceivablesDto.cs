using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Dto.Reports
{
    /// <summary>
    /// Query parameters for filtering outstanding receivables report
    /// </summary>
    public class OutstandingReceivablesQueryDto
    {
        /// <summary>
        /// Filter by specific customer ID (optional)
        /// </summary>
        public int? CustomerId { get; set; }

        

        /// <summary>
        /// Filter invoices with due date from this date (optional)
        /// </summary>
        public DateTime? DueDateFrom { get; set; }

        /// <summary>
        /// Filter invoices with due date until this date (optional)
        /// </summary>
        public DateTime? DueDateTo { get; set; }

        /// <summary>
        /// Filter only overdue invoices (optional)
        /// </summary>
        public bool? OnlyOverdue { get; set; }

        /// <summary>
        /// Minimum outstanding amount to include (optional)
        /// </summary>
        public decimal? MinimumAmount { get; set; }

        /// <summary>
        /// Sort field: DueDate, RemainingBalance, DaysOverdue, CustomerName (default: DueDate)
        /// </summary>
        public string? SortBy { get; set; } = "DueDate";

        /// <summary>
        /// Sort direction: asc or desc (default: asc)
        /// </summary>
        public string? SortDirection { get; set; } = "asc";


        /// <summary>
        /// Gets or sets the payment method used for the transaction.
        /// </summary>
        public string? PaymentMethod { get; set; }

        public bool IsValidSortField()
        {
            var allowedFields = new HashSet<string> { "duedate", "remainingbalance", "daysoverdue", "customername" ,"customeremail"};
            return string.IsNullOrWhiteSpace(SortBy) || 
                   allowedFields.Contains(SortBy.ToLower());
        }

        public bool IsValidSortDirection()
        {
            var allowedDirections = new HashSet<string> { "asc", "desc" };
            return string.IsNullOrWhiteSpace(SortDirection) || 
                   allowedDirections.Contains(SortDirection.ToLower());
        }
    }

    /// <summary>
    /// Represents a single line item in the outstanding receivables report
    /// </summary>
    public class OutstandingReceivableItemDto
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal RemainingBalance { get; set; }
        public int DaysOverdue { get; set; }
    }

    /// <summary>
    /// Complete outstanding receivables report with summary statistics
    /// </summary>
    public class OutstandingReceivablesReportDto
    {
        /// <summary>
        /// List of all outstanding invoices
        /// </summary>
        public List<OutstandingReceivableItemDto> Items { get; set; } = new();

        /// <summary>
        /// Report summary statistics
        /// </summary>
        public OutstandingReceivablesSummaryDto Summary { get; set; } = new();

        /// <summary>
        /// Report generation timestamp
        /// </summary>
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Summary statistics for outstanding receivables
    /// </summary>
    public class OutstandingReceivablesSummaryDto
    {
        /// <summary>
        /// Total number of outstanding invoices
        /// </summary>
        public int TotalInvoices { get; set; }

        /// <summary>
        /// Total outstanding amount across all invoices
        /// </summary>
        public decimal TotalOutstanding { get; set; }

        /// <summary>
        /// Number of overdue invoices
        /// </summary>
        public int OverdueInvoices { get; set; }

        /// <summary>
        /// Total amount from overdue invoices
        /// </summary>
        public decimal OverdueAmount { get; set; }

        /// <summary>
        /// Number of unique customers with outstanding balances
        /// </summary>
        public int TotalCustomers { get; set; }

        /// <summary>
        /// Average days overdue (for overdue invoices only)
        /// </summary>
        public double AverageDaysOverdue { get; set; }

     
       
    }
}
