using BillingInvoicingPlatform.Application.Service.Abstraction;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Infrastructure.BackgroundJobs
{
    /// <summary>
    /// Lightweight infrastructure job that Hangfire / scheduler will call.
    /// It resolves and invokes application-level overdue business logic via IInvoiceOverdueService.
    /// </summary>
    public class InvoiceOverdueJob
    {
        private readonly IInvoiceOverdueService _overdueService;
        private readonly ILogger<InvoiceOverdueJob> _logger;
        public InvoiceOverdueJob(IInvoiceOverdueService overdueService, ILogger<InvoiceOverdueJob> logger)
        {
            _overdueService = overdueService;
            _logger = logger;
        }


        public async Task ExecuteJobAsync()
        {
            _logger.LogInformation("InvoiceOverdueJob started at {Time} UTC", DateTime.UtcNow);

                 await _overdueService.ExecuteAsync();
                _logger.LogInformation("InvoiceOverdueJob finished at {Time} UTC", DateTime.UtcNow);
            
            
        }
    }
}
