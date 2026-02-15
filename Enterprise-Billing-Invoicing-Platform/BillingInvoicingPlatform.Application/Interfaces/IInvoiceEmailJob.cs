using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Interfaces
{
    public interface IInvoiceEmailJob
    {
        Task SendInvoiceEmailJobAsync(int invoiceId);
    }
}
