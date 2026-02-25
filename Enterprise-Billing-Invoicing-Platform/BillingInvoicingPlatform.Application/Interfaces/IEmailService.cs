using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Interfaces
{
    public interface IEmailService
    {
       
        Task SendInvoiceEmailAsync(string recipientEmail, string recipientName, string invoiceNumber, decimal totalAmount, DateTime issueDate, DateTime dueDate, byte[] pdfAttachment);
    }
}
