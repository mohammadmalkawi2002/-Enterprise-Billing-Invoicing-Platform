using BillingInvoicingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Interfaces
{
    public interface IInvoicePdfService
    {
        /// <summary>
        /// Generates PDF invoice as byte array
        /// </summary>
        /// <param name="invoice">Invoice entity with all related data</param>
        /// <returns>PDF document as byte array</returns>
       Task< byte[]> GenerateInvoicePdfAsync(Invoice invoice);
    }
}
