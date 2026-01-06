using BillingInvoicingPlatform.Application.Dto.Invoice;
using BillingInvoicingPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Service.Abstraction
{
    public interface IInvoiceService
    {
        Task<InvoiceDto> CreateAsync(CreateInvoiceDto dto);
        Task UpdateAsync(int invoiceId, UpdateInvoiceDto dto);
        Task DeleteInvoice(int invoiceId);
     
        Task<InvoiceDto> GetInvoiceWithDetailsAsync(int invoiceId);

        /// <summary>
        /// Changes invoice status with routing logic.
        /// Automatically calls SendInvoiceAsync or CancelInvoiceAsync based on requested status.
        /// Supports: Sent, Cancelled statuses only.
        /// </summary>
        Task<InvoiceDto> ChangeInvoiceStatusAsync(int invoiceId, InvoiceStatus newStatus);

        /// <summary>
        /// Sends an invoice from Draft status to Sent status.
        /// Validates that invoice has at least one item.
        /// </summary>
        Task<InvoiceDto> SendInvoiceAsync(int invoiceId);

        /// <summary>
        /// Cancels an invoice.
        /// Validates that invoice is not Paid/PartiallyPaid and has no recorded payments.
        /// </summary>
        Task<InvoiceDto> CancelInvoiceAsync(int invoiceId);
    }
}
