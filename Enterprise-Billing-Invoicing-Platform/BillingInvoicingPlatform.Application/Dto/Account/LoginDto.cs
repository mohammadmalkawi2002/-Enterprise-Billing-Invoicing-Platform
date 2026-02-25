using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Dto.Account
{
    public class LoginDto
    {
        // The identifier can be either username or email:
        public string? Identifier { get; set; }
        public string? Password { get; set; }
    }
}
