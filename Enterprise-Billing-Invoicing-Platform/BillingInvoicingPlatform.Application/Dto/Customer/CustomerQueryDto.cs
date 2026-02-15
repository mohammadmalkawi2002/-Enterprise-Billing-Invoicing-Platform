using BillingInvoicingPlatform.Application.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Dto.Customer
{
    /// <summary>
    /// Used for querying customers with pagination.
    /// Fields for filtering,Sorting and Searching can be added as needed.
    /// 
    /// </summary>
    public class CustomerQueryDto:PaginationRequestParameters
    {
      
        public string? SearchBy { get; set; }

        public string? SortBy { get; set; } = "id";
        public string? SortDirection { get; set; } = "asc";

        public string? Status { get; set; }


    }
}
