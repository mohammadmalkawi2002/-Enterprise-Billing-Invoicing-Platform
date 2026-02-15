using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends invoice email with PDF attachment to customer
        /// </summary>
        /// <param name="recipientEmail">Customer email address</param>
        /// <param name="recipientName">Customer name</param>
        /// <param name="invoiceNumber">Invoice number (e.g., INV-2025-00001)</param>
        /// <param name="totalAmount">Total invoice amount</param>
        /// <param name="issueDate">Invoice issue date</param>
        /// <param name="dueDate">Invoice due date</param>
        /// <param name="pdfAttachment">Invoice PDF as byte array</param>
        Task SendInvoiceEmailAsync(string recipientEmail, string recipientName, string invoiceNumber, decimal totalAmount, DateTime issueDate, DateTime dueDate, byte[] pdfAttachment);
    }
}
