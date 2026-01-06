using BillingInvoicingPlatform.Application.Dto.Invoice;
using BillingInvoicingPlatform.Application.Interfaces;
using BillingInvoicingPlatform.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Validators
{
    public class InvoiceValidator:AbstractValidator<CreateInvoiceDto>
    {
      
        public InvoiceValidator()
        {

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).
                WithMessage("CustomerId must be greater than 0.");

            RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));


            // Issue Date Cannot Be In Future
            RuleFor(x => x.IssueDate)
                .NotNull()
                .WithMessage("Issue Date is required.")
                .Must(issueDate => issueDate.Value.Date <= DateTime.UtcNow.Date)
                .WithMessage("Issue Date cannot be in the future.");

            // Due Date
            RuleFor(x => x.DueDate)
                .NotNull()
                .WithMessage("Due Date is required.")
                .Must((dto, dueDate) =>
                    dueDate!.Value.Date >= dto.IssueDate!.Value.Date)
                .WithMessage("Due Date must be on or after the Issue Date.")
                .Must((dto, dueDate) =>
                    (dueDate!.Value - dto.IssueDate!.Value).TotalDays <= 365)
                .WithMessage("Due Date cannot exceed 365 days from the Issue Date.");


            //new updated: Invoice must have at least one item

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("Invoice must have at least one item.");

           
            RuleForEach(x => x.Items)
            .SetValidator(new CreateInvoiceItemValidator());

        }

      
    }


    public class UpdateInvoiceValidator : AbstractValidator<UpdateInvoiceDto>
    {
        public UpdateInvoiceValidator()
        {
            RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));

            // Issue Date Cannot Be In Future
            RuleFor(x => x.IssueDate)
                .NotNull()
                .WithMessage("Issue Date is required.")
                .Must(issueDate => issueDate.Value.Date <= DateTime.UtcNow.Date)
                .WithMessage("Issue Date cannot be in the future.");

            // Due Date
            RuleFor(x => x.DueDate)
                .NotNull()
                .WithMessage("Due Date is required.")
                .Must((dto, dueDate) =>
                    dueDate!.Value.Date >= dto.IssueDate!.Value.Date)
                .WithMessage("Due Date must be on or after the Issue Date.")
                .Must((dto, dueDate) =>
                    (dueDate!.Value - dto.IssueDate!.Value).TotalDays <= 365)
                .WithMessage("Due Date cannot exceed 365 days from the Issue Date.");

            // Items
            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("Invoice must have at least one item.");


            RuleForEach(x => x.Items)
          .SetValidator(new UpdateInvoiceItemValidator());
        }
    }

    public class CreateInvoiceItemValidator : AbstractValidator<CreateInvoiceItemDto>
    {
        public CreateInvoiceItemValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .Length(5, 500)
                .WithMessage("Description must be between 5 and 500 characters.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");
            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Unit Price must be non-negative.");

            RuleFor(x => x.TaxRate)
                .NotNull()
                .WithMessage("Tax Rate is required.")
                .InclusiveBetween(0, 100)
                .WithMessage("Tax Rate must be between 0 and 100.");
        }
    }


    public class UpdateInvoiceItemValidator : AbstractValidator<UpdateInvoiceItemDto>
    {
        public UpdateInvoiceItemValidator()
        {
            When(x => x.Id.HasValue, () =>
            {
                RuleFor(x => x.Id!.Value)
                    .GreaterThan(0)
                    .WithMessage("Item Id must be greater than 0 when provided.");
            });


            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .Length(5, 500)
                .WithMessage("Description must be between 5 and 500 characters.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Unit Price must be non-negative.");

            RuleFor(x => x.TaxRate)
                .NotNull()
                .WithMessage("Tax Rate is required.")
                .InclusiveBetween(0, 100)
                .WithMessage("Tax Rate must be between 0 and 100.");
        }
    }

    public class ChangeInvoiceStatusValidator : AbstractValidator<ChangeStatusRequest>
    {
        public ChangeInvoiceStatusValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty()
                .Must(status => status == InvoiceStatus.Sent || status == InvoiceStatus.Cancelled)
                .WithMessage("Invalid invoice status.Only 'Sent' and 'Cancelled' statuses are allowed.");
                


        }
    }


}
