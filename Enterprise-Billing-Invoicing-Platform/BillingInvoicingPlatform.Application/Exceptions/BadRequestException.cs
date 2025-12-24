using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Exceptions
{
    public class BadRequestException:Exception
    {
        public BadRequestException():base()
        {
            
        }

        public BadRequestException(string message, Exception ?innerException) :base(message,innerException)
        {
            
        }
    }
}
