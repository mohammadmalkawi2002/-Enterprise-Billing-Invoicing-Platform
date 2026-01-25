using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Infrastructure.Configuration
{
    /// <summary>
    /// Email configuration settings loaded from appsettings.json
    /// Maps to "MailSettings" section in appsettings.json
    /// </summary>
    public class MailSettings
    {
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EnableSsl { get; set; } = true;
    }
}

