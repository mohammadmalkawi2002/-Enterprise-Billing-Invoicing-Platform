using BillingInvoicingPlatform.Application.Exceptions;
using BillingInvoicingPlatform.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Infrastructure.BackgroundJobs
{
    /// <summary>
    /// Hangfire background job for sending invoice emails
    /// This job is executed asynchronously after invoice status changes to "Sent"
    /// Used for Orchestration of email sending
    /// Load Data,Html email body construction, pdf attachment ,Actual email sending is handled in IEmailService
    /// </summary>
    public class InvoiceEmailJob:IInvoiceEmailJob
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IEmailService _emailService;
        private readonly IInvoicePdfService _pdfService;
        private readonly ILogger<InvoiceEmailJob> _logger;

        public InvoiceEmailJob(IInvoiceRepository invoiceRepository, IEmailService emailService, IInvoicePdfService pdfService, ILogger<InvoiceEmailJob> logger)
        {
            _invoiceRepository = invoiceRepository;
            _emailService = emailService;
            _pdfService = pdfService;
            _logger = logger;
        }


        public async Task SendInvoiceEmailJobAsync(int invoiceId)
        {
            try
            {
                _logger.LogInformation(
                    "📧 Email job started for invoice ID {InvoiceId} at {Time}",
                    invoiceId,
                    DateTime.UtcNow
                );

                //1] Load invoice with  details:
                var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
                if (invoice is null)
                {
                    _logger.LogError("Invoice with ID {InvoiceId} not found. Email job aborted.", invoiceId);   
                    return;
                }


                //2] Generate PDF attachment:
                _logger.LogInformation(
                     "📄 Generating PDF for invoice {InvoiceNumber}",
                     invoice.InvoiceNumber
                 );

                var pdfBytes = await _pdfService.GenerateInvoicePdfAsync(invoice);

                // 3] Send email with PDF attachment

                _logger.LogInformation(
                    "✉️ Sending invoice email to {CustomerEmail} for invoice {InvoiceNumber}",
                    invoice.Customer.Email,
                    invoice.InvoiceNumber
                );

                await _emailService.SendInvoiceEmailAsync(
                    recipientEmail: invoice.Customer.Email,
                    recipientName: invoice.Customer.Name,
                    invoiceNumber: invoice.InvoiceNumber,
                    totalAmount: invoice.TotalAmount,
                    issueDate: invoice.IssueDate ?? DateTime.UtcNow,
                    dueDate: invoice.DueDate ?? DateTime.UtcNow.AddDays(30),
                    pdfAttachment: pdfBytes
                );

                _logger.LogInformation(
                   "✅ Email sent successfully for invoice {InvoiceNumber} at {Time}",
                   invoice.InvoiceNumber,
                   DateTime.UtcNow);


            }

            catch (Exception ex)
            {
                _logger.LogError(ex,
                     "❌ Failed to send email for invoice ID {InvoiceId}. Error: {Error}",
                     invoiceId,
                     ex.Message
                 );
                throw; // Hangfire will automatically retry
            }
        }
    }
}
