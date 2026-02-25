using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Dto.Account
{
    public class AuthResponse
    {
        public string? Message { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }


        /// <summary>
        /// by default, when the user is authenticated, this property will be set to true. otherwise, it will be false.
        /// </summary>
        public bool IsAuthenticated { get; set; }
        public string? Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public List<string>? Roles { get; set; }
    }
}



