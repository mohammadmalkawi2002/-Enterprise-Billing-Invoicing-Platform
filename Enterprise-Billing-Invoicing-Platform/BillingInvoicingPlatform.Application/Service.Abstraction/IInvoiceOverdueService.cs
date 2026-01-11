using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Service.Abstraction
{
    /// <summary>
    /// Application-level service that encapsulates overdue invoice business logic.
    /// The Background Job (in Infrastructure) will call this service.
    /// </summary>
    public interface IInvoiceOverdueService
    {


        /// <summary>
        /// Find candidate invoices and apply overdue business rules:
        /// - If fully paid => set Paid
        /// - If Sent and overdue => set Overdue
        /// - If PartiallyPaid and overdue => keep PartiallyPaid but update DaysOverdue (entity computed)
        /// Persist changes to repository.
        /// </summary>
        Task ExecuteAsync();
    }
}
