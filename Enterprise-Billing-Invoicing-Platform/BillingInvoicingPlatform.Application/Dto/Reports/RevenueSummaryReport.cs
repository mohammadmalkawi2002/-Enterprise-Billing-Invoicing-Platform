using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Dto.Reports
{
    
   
    public class RevenueSummaryQueryDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        
        /// <summary>
        /// Filter by specific customer ID
        /// </summary>

        public int ? CustomerId { get; set; }
       

       
        /// <summary>
        /// Gets or sets the maximum number of top customers to include in the results.
        /// </summary>
        public int ?TopCustomers { get; set; }

     

    }


    public class RevenueSummaryDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalInvoices { get; set; }
        public decimal AverageInvoiceValue { get; set; }

        public int TotalCustomers { get; set; }

        public decimal PaidRevenue { get; set; }
        public decimal PartiallyPaidRevenue { get; set; }
        public decimal UnpaidRevenue { get; set; }
      

    }



    public class CustomerRevenueDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalRevenue { get; set; }
        public int InvoiceCount { get; set; }
        public decimal PercentageOfTotal { get; set; }
    }
    public class RevenueSummaryReportDto
    {


        public RevenueSummaryDto Summary { get; set; }
        public List<CustomerRevenueDto> RevenueByCustomer { get; set; }
    
        public DateTime GeneratedAt { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}
