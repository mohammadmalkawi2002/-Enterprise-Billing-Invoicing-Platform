using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Domain.Enums
{
    public enum PaymentMethod
    {
        Cash=1,
      CreditCard ,
     DebitCard  ,
     BankTransfer,
         Check,
        PayPal,
        Other
    }
}
