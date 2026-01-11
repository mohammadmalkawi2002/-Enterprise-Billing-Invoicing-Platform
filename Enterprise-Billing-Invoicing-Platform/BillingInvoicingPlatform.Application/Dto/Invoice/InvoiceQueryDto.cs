using BillingInvoicingPlatform.Application.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Dto.Invoice
{
    public class InvoiceQueryDto:PaginationRequestParameters
    {
        /// <summary>
        /// Search across InvoiceNumber, CustomerName, Status
        /// </summary>
        public string? SearchBy { get; set; }

        /// <summary>
        /// Filter by invoice status (Draft, Sent, Paid, etc.)
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Sort field: CreatedAt, InvoiceNumber (validates against whitelist)
        /// </summary>
        public string? SortBy { get; set; } = "CreatedAt";

        /// <summary>
        /// asc or desc
        /// </summary>
        public string? SortDirection { get; set; } = "asc";


        // Allowed sort fields for validation
        private static readonly HashSet<string> AllowedSortFields = new(StringComparer.OrdinalIgnoreCase)
        {
            "CreatedAt",
            "InvoiceNumber"
           
        };

        /// <summary>
        /// Validates SortBy against whitelist to prevent injection
        /// </summary>
        public bool IsValidSortField() => AllowedSortFields.Contains(SortBy ?? "");

        /// <summary>
        /// Validates SortDirection is asc or desc
        /// </summary>
        public bool IsValidSortDirection() =>
            SortDirection?.Equals("asc", StringComparison.OrdinalIgnoreCase) == true ||
            SortDirection?.Equals("desc", StringComparison.OrdinalIgnoreCase) == true;
    }
}
