using BillingInvoicingPlatform.Application.Interfaces;
using BillingInvoicingPlatform.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;

namespace BillingInvoicingPlatform.Infrastructure.ExternalService
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger _logger;
        public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }
        public async Task SendInvoiceEmailAsync(string recipientEmail, string recipientName, string invoiceNumber, decimal totalAmount, DateTime issueDate, DateTime dueDate, byte[] pdfAttachment)
        {
            try
            {
                _logger.LogInformation(
                         "Starting email sending process for invoice {InvoiceNumber} to {recipientEmail}",
                         invoiceNumber,
                         recipientEmail);


                // STEP 1: CREATE EMAIL MESSAGE:

                var message = new MimeMessage()
                {
                    Sender = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail),
                };

                // Set sender:
                message.From.Add(new MailboxAddress
                    (
                      _mailSettings.SenderName
                        , _mailSettings.SenderEmail
                    ));

                // Set recipient:
                message.To.Add(new MailboxAddress(recipientName, recipientEmail));

                //Set Subject:
                message.Subject = $"Invoice {invoiceNumber} From {_mailSettings.SenderName}";




                // STEP 2: Build EMAIL BODY (HTML) :

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
        <!DOCTYPE html>
         <html>
          <head>
         <style>
            body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            color: #333;
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
        }}
        .header {{
            background-color: #2196F3;
            color: white;
            padding: 20px;
            text-align: center;
            border-radius: 5px 5px 0 0;
        }}
        .content {{
            background-color: #f9f9f9;
            padding: 30px;
            border: 1px solid #ddd;
        }}
        .invoice-details {{
            background-color: white;
            padding: 20px;
            margin: 20px 0;
            border-radius: 5px;
            border: 1px solid #e0e0e0;
        }}
        .detail-row {{
            display: flex;
            justify-content: space-between;
            padding: 10px 0;
            border-bottom: 1px solid #eee;
        }}
        .detail-label {{
            font-weight: bold;
            color: #555;
        }}
        .detail-value {{
            color: #333;
        }}
        .total-amount {{
            font-size: 24px;
            color: #2E7D32;
            font-weight: bold;
        }}
        .footer {{
            background-color: #f5f5f5;
            padding: 20px;
            text-align: center;
            font-size: 12px;
            color: #666;
            border-radius: 0 0 5px 5px;
        }}
         </style>
             </head>
            <body>
    <div class='header'>
        <h1>📧 New Invoice</h1>
        </div>
    
        <div class='content'>
        <p>Dear <strong>{recipientName}</strong>,</p>
        
        <p>Thank you for your business! Please find attached your invoice <strong>{invoiceNumber}</strong>.</p>
        
          <div class='invoice-details'>
            <h2>Invoice Details</h2>
            
            <div class='detail-row'>
                <span class='detail-label'>Invoice Number:</span>
                <span class='detail-value'>{invoiceNumber}</span>
            </div>
            
            <div class='detail-row'>
                <span class='detail-label'>Issue Date:</span>
                <span class='detail-value'>{issueDate:MMMM dd, yyyy}</span>
            </div>
            
            <div class='detail-row'>
                <span class='detail-label'>Due Date:</span>
                <span class='detail-value'>{dueDate:MMMM dd, yyyy}</span>
            </div>
            
            <div class='detail-row' style='border-bottom: none; margin-top: 15px;'>
                <span class='detail-label'>Total Amount:</span>
                <span class='total-amount'>${totalAmount:N2}</span>
            </div>
        </div>
        
        <p><strong>Payment Instructions:</strong></p>
        <ul>
            <li>Please remit payment by <strong>{dueDate:MMMM dd, yyyy}</strong></li>
            <li>The invoice is attached as a PDF document</li>
            <li>For any questions, please reply to this email</li>
        </ul>
        
        <p>We appreciate your prompt payment and continued business!</p>
        
        <p>Best regards,<br/>
        <strong>{_mailSettings.SenderName}</strong></p>
          </div>
    
     <div class='footer'>
        <p>This is an automated message from {_mailSettings.SenderName}</p>
        <p>© {DateTime.UtcNow.Year} All rights reserved</p>
    </div>
    </body>
    </html>"
                };



                // STEP 3: ATTACH PDF INVOICE:

                bodyBuilder.Attachments.Add(
                    $"Invoice {invoiceNumber}.pdf"
                    , pdfAttachment
                    , ContentType.Parse("application/pdf"
                    ));

                message.Body = bodyBuilder.ToMessageBody();



                // STEP 4: SEND EMAIL VIA SMTP (THIS ACTUALLY SENDS IT)

                using var smtpClient = new SmtpClient();

                _logger.LogInformation(
                      "Connecting to SMTP Server {Host}: {Port}"
                        , _mailSettings.SmtpHost, _mailSettings.SmtpPort
                        );
                // Connect to SMTP server :

                await smtpClient.ConnectAsync(
                    _mailSettings.SmtpHost
                    , _mailSettings.SmtpPort
                    , _mailSettings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None

                    );

                // Authenticate :

                await smtpClient.AuthenticateAsync(
                    _mailSettings.SenderEmail
                    , _mailSettings.Password

                    );

                _logger.LogInformation(
                        " Sending email to {Email} for invoice {InvoiceNumber}",
                        recipientEmail,
                        invoiceNumber
                    );

                // Actually Sends  The EMAIL :
                await smtpClient.SendAsync(message);

                // Disconnect
                await smtpClient.DisconnectAsync(true);

                _logger.LogInformation(
                    "✅ Email sent successfully to {Email}",
                    recipientEmail
                );

            }

            catch(Exception ex) 
            {
                _logger.LogError(ex,
                        "❌ Failed to send email to {Email} for invoice {InvoiceNumber}. Error: {Error}",
                        recipientEmail,
                        invoiceNumber,
                        ex.Message
                    );
                throw;
            }
            
        }
    }
}
