using BillingInvoicingPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string? PhotoUrl { get; set; }
        public string PasswordHash { get; set; }

        public UserRole Role { get; set; }

    }
}
