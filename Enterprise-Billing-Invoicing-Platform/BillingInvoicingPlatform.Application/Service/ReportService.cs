using BillingInvoicingPlatform.Application.Dto.Invoice;
using BillingInvoicingPlatform.Application.Dto.Reports;
using BillingInvoicingPlatform.Application.Exceptions;
using BillingInvoicingPlatform.Application.Interfaces;
using BillingInvoicingPlatform.Application.Service.Abstraction;
using BillingInvoicingPlatform.Domain.Entities;
using BillingInvoicingPlatform.Domain.Enums;
using FluentValidation.Validators;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Service
{
    public class ReportService : IReportService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ILogger<ReportService> _logger;

        public ReportService(
            IInvoiceRepository invoiceRepository,
            ILogger<ReportService> logger)
        {
            _invoiceRepository = invoiceRepository;
            _logger = logger;
        }

        public async Task<OutstandingReceivablesReportDto> GetOutstandingReceivablesReportAsync(
            OutstandingReceivablesQueryDto query)
        {
            _logger.LogInformation("Generating Outstanding Receivables Report with filters: {@Query}", query);

            // Validate query parameters:
            ValidateOutStandingReceivablesQuery(query);
          
            // Get outstanding receivables from repository
            var items = await _invoiceRepository.GetOutstandingReceivablesAsync(query);

            // Calculate summary statistics
            var summary = CalculateOutStandingSummary(items);

            var report = new OutstandingReceivablesReportDto
            {
                 Items=items,
                  Summary=summary,
                  GeneratedAt=DateTime.UtcNow

            };

            _logger.LogInformation(
                "Outstanding Receivables Report generated: {TotalInvoices} invoices, ${TotalOutstanding} outstanding",
                summary.TotalInvoices,
                summary.TotalOutstanding);

            return report;
        }




        private OutstandingReceivablesSummaryDto CalculateOutStandingSummary(List<OutstandingReceivableItemDto> items)
        {
            if (!items.Any())
            {
                return new OutstandingReceivablesSummaryDto
                {
                    TotalInvoices = 0,
                    TotalOutstanding = 0,
                    OverdueInvoices = 0,
                    OverdueAmount = 0,
                    TotalCustomers = 0,
                    AverageDaysOverdue = 0
                };
            }

            //TODO:Note its redundant because all items 
            var overdueItems = items.Where(i => i.DaysOverdue > 0).ToList();

            return new OutstandingReceivablesSummaryDto
            {
                TotalInvoices = items.Count,
                TotalOutstanding = Math.Round(items.Sum(i => i.RemainingBalance), 2),
                OverdueInvoices = overdueItems.Count,
                OverdueAmount = Math.Round(overdueItems.Sum(i => i.RemainingBalance), 2),
                TotalCustomers = items.Select(i => i.CustomerId).Distinct().Count(),
                AverageDaysOverdue = overdueItems.Any()
                    ? Math.Round(overdueItems.Average(i => i.DaysOverdue), 1)
                    : 0
            };
        }


        public async Task<RevenueSummaryReportDto> GetRevenueSummaryReportAsync(RevenueSummaryQueryDto query)
        {
            _logger.LogInformation("Generating Revenue Summary Report with filters: {@Query}", query);

            //1] Validate query parameters :

            ValidateRevenueSummaryQuery(query);

            //2] Get revenue invoices from repository :

            var invoices = await _invoiceRepository.GetRevenueDataAsync(query);

            //3] Calculate summary metrics:

            var summary = CalculateRevenueSummary(invoices);

            //4]Calculate customer breakdown:

            var revenueByCustomer = CalculateRevenueByCustomer(invoices, summary.TotalRevenue, query.TopCustomers);

           


            //6]Build report DTO:

            return new RevenueSummaryReportDto 
            { 
                Summary=summary,
                RevenueByCustomer=revenueByCustomer,
                 GeneratedAt=DateTime.UtcNow,
                PeriodStart=query.StartDate.Value,
                PeriodEnd=query.EndDate.Value,
                 
                 
                 
            };
        }

     


        private void ValidateOutStandingReceivablesQuery(OutstandingReceivablesQueryDto query) 
        {
            if (!query.IsValidSortField())
                throw new BusinessException("Invalid SortBy field. Allowed: DueDate, RemainingBalance, DaysOverdue, CustomerName");

            if (!query.IsValidSortDirection())
                throw new BusinessException("Invalid SortDirection. Allowed: asc, desc");

            // Validate date range
            if (query.DueDateFrom.HasValue && query.DueDateTo.HasValue &&
                query.DueDateFrom.Value > query.DueDateTo.Value)
            {
                throw new BusinessException("DueDateFrom cannot be greater than DueDateTo");
            }


        }



        #region Revenue Summary Helper Methods




        private void ValidateRevenueSummaryQuery(RevenueSummaryQueryDto query)
        {
            // Validate required fields
            


            // Validate date range is not too large (optional business rule)
            var daysDifference = (query.EndDate.Value - query.StartDate.Value).Days;
            if (daysDifference > 365)
            {
                _logger.LogWarning("Large date range requested: {Days} days", daysDifference);
                throw new BusinessException("Date range cannot exceed 365 days");
            }
           

            // Validate TopCustomers if provided
            if (query.TopCustomers.HasValue && query.TopCustomers.Value <= 0)
            {
                throw new BusinessException("TopCustomers must be greater than 0");
            }
        }

        private RevenueSummaryDto CalculateRevenueSummary(List<InvoiceDto> items) 
        {
            // Handle empty case
            if (items == null || !items.Any())
            {
                _logger.LogInformation("No invoices found for revenue calculation");
                return new RevenueSummaryDto
                {
                    TotalRevenue = 0,
                    TotalInvoices = 0,
                    AverageInvoiceValue = 0,
                    TotalCustomers = 0,
                    PaidRevenue = 0,
                    PartiallyPaidRevenue = 0,
                    UnpaidRevenue = 0
                };
            }

            var totalRevenue=items.Sum(i=>i.TotalAmount);
            var totalInvoices=items.Count;


            return new RevenueSummaryDto 
            { 
                 TotalRevenue=Math.Round(totalRevenue,2),
                 TotalInvoices=totalInvoices,
                 AverageInvoiceValue= totalInvoices > 0 ? Math.Round(totalRevenue / totalInvoices, 2) : 0,
                 TotalCustomers=items.Select(i=>i.CustomerId).Distinct().Count(),

                // Revenue breakdown by status
                PaidRevenue = Math.Round(items.Where(i=>i.Status.Equals(InvoiceStatus.Paid))
                                            .Sum(i=>i.TotalAmount),2),
                PartiallyPaidRevenue=Math.Round(items.Where(i=>i.Status.Equals(InvoiceStatus.PartiallyPaid))
                                            .Sum(i=>i.TotalAmount),2),
                UnpaidRevenue=Math.Round(items.Where(i=>i.Status.Equals(InvoiceStatus.Sent)||
                                                      i.Status.Equals(InvoiceStatus.Overdue))
                                            .Sum(i=>i.TotalAmount),2),
                
                  
                 
                 
                 
            };



          






                    
           
        }

        private List< CustomerRevenueDto> CalculateRevenueByCustomer(List<InvoiceDto> invoices, 
                        decimal totalRevenue, int? topCustomers
            )
        {

            // Handle empty case

            if(invoices == null || !invoices.Any())
            {
               return new List<CustomerRevenueDto>();
            }
            // Group by customer and calculate revenue

            var customerRevenue = invoices
                                            .GroupBy(i => new { i.CustomerId, i.CustomerName })
                                            .Select(g => new CustomerRevenueDto
                                            {
                                                 CustomerId=g.Key.CustomerId,
                                                 CustomerName=g.Key.CustomerName,
                                                  TotalRevenue=Math.Round(g.Sum(i=>i.TotalAmount),2),
                                                    InvoiceCount=g.Count(),
                                                    PercentageOfTotal= totalRevenue > 0 ? Math.Round((g.Sum(i=>i.TotalAmount) / totalRevenue) * 100, 2) : 0


                                            }).OrderByDescending(c=>c.TotalRevenue)
                                            .ToList();

            //// Apply top N filter if requested:
            if (topCustomers.HasValue) 
            {
                customerRevenue = customerRevenue.Take(topCustomers.Value).ToList();
                _logger.LogInformation("Filtered to top {Count} customers", topCustomers.Value);

            }

           return customerRevenue;
        }

       



        #endregion
    }
}
