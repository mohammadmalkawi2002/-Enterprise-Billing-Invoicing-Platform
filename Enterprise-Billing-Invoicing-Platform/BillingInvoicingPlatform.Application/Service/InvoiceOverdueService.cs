using BillingInvoicingPlatform.Application.Exceptions;
using BillingInvoicingPlatform.Application.Interfaces;
using BillingInvoicingPlatform.Application.Service.Abstraction;
using BillingInvoicingPlatform.Domain.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Service
{
    public class InvoiceOverdueService : IInvoiceOverdueService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ILogger<InvoiceOverdueService> _logger;

        public InvoiceOverdueService(IInvoiceRepository invoiceRepository,ILogger<InvoiceOverdueService> logger)
        {
            _invoiceRepository = invoiceRepository;
            _logger = logger;
        }
        public async Task ExecuteAsync()
        {
            _logger.LogInformation("InvoiceOverdueService.ExecuteAsync started at {Time} UTC", DateTime.UtcNow);

            //1] Get the overDueInvoices from DB:
            var overDueInvoices = await _invoiceRepository.GetOverdueInvoicesAsync();
            if (!overDueInvoices.Any())
            {
                _logger.LogInformation("No overdue invoices found");

                return;
            }


            foreach (var invoice in overDueInvoices) 
            {
               

                try
                {
                    //TODO:Check redundant condition

                    // 1] if (invoice.Status == InvoiceStatus.Paid)
                    //    continue;

                    ////Not overDue:
                    //2] if (invoice.DaysOverdue <= 0)
                    //    continue;

                    //Rule:Sent=> Overdue:
                    if (invoice.Status == InvoiceStatus.Sent)
                    {
                        invoice.Status = InvoiceStatus.Overdue;
                        invoice.UpdatedAt = DateTime.UtcNow;

                        _logger.LogInformation("Invoice {InvoiceId} changed sent-> Overdue (DaysOverdue={Days}).", invoice.Id, invoice.DaysOverdue);
                    }


                    // ==>Partially Paid Goes Overdue
                    if (invoice.Status == InvoiceStatus.PartiallyPaid)
                    {
                        invoice.Status = InvoiceStatus.Overdue;
                        invoice.UpdatedAt = DateTime.UtcNow;
                        _logger.LogInformation("Invoice {InvoiceId} changed pariallyPaid-> Overdue (DaysOverdue={Days}).", invoice.Id, invoice.DaysOverdue);

                    }


                 

                }
                catch (Exception ex) 
                {
                    _logger.LogError(ex, "Failed to process overdue invoice Id={InvoiceId}", invoice?.Id);
                    
                }



            }

            //Single SaveChanges :
             await _invoiceRepository.SaveChangesAsync();

            _logger.LogInformation("InvoiceOverdueService.ExecuteAsync completed at {Time} UTC", DateTime.UtcNow);


        }


    }
}
