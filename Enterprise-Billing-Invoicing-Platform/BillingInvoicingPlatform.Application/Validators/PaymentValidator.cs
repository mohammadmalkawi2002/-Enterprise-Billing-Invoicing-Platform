using BillingInvoicingPlatform.Application.Dto.Payment;
using BillingInvoicingPlatform.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Validators
{
    public class PaymentValidator:AbstractValidator<CreatePaymentDto>
    {
        public PaymentValidator()
        {
            RuleFor(p => p.InvoiceId)
                .NotEmpty()
                .WithMessage("InvoiceId is required")
                .GreaterThan(0)
                .WithMessage("InvoiceId must be greater than 0.");

            RuleFor(p=>p.PaymentAmount)
              .GreaterThan(0)
                .WithMessage("Payment amount must be greater than 0");

            RuleFor(x => x.PaymentDate)
                .NotEmpty()
                .WithMessage("Payment date is required")
                .Must(paymentDate => paymentDate.Date <= DateTime.UtcNow.Date)
                .WithMessage("Payment date cannot be in the future");

            RuleFor(x => x.PaymentMethod)
                .NotEmpty()
                .WithMessage("Payment method is required")
                .Must(BeAValidPaymentMethod)
                .WithMessage("Invalid payment method. Valid options: Cash, CreditCard, DebitCard, BankTransfer, Check, PayPal, Other");
        }

        private bool BeAValidPaymentMethod(string paymentMethod)
        {
            if (string.IsNullOrWhiteSpace(paymentMethod))
                return false;

            return Enum.TryParse<PaymentMethod>(paymentMethod, ignoreCase: true, out _);
        }
    }
}
