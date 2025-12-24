using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Domain.Enums
{
    
    public enum InvoiceStatus
    {
        Draft=1,
        Sent,
        PartiallyPaid,
        Paid,
        Cancelled,
        Overdue


    }
}
