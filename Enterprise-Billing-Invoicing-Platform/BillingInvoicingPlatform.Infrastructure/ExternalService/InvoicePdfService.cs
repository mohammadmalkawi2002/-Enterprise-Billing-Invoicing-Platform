using BillingInvoicingPlatform.Application.Interfaces;
using BillingInvoicingPlatform.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Infrastructure.ExternalService
{
    /// <summary>
    ///Using External dependency QuestPDF:
    /// </summary>
    public class InvoicePdfService : IInvoicePdfService
    {
        public async Task<byte[]> GenerateInvoicePdfAsync(Invoice invoice)
        {
            // Set QuestPDF license (Community license is free):
            QuestPDF.Settings.License = LicenseType.Community;

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(50);
                    page.PageColor(Colors.White);

                    page.Header().Element(header => ComposeHeader(header, invoice));
                    page.Content().Element(content => ComposeContent(content, invoice));
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Page ");
                        text.CurrentPageNumber();
                        text.Span(" of ");
                        text.TotalPages();
                    });
                });
            }).GeneratePdf();

            return Task.FromResult(pdfBytes).Result;

        }


        private void ComposeHeader(IContainer container, Invoice invoice)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("INVOICE").FontSize(28).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Text("Billing & Invoicing System").FontSize(14);
                    column.Item().PaddingTop(5).Text("Email: invoicebillingplatform@gmail.com").FontSize(10);
                });

                row.RelativeItem().AlignRight().Column(column =>
                {
                    column.Item().Text($"Invoice #: {invoice.InvoiceNumber}").FontSize(12).Bold();
                    column.Item().Text($"Date: {DateTime.UtcNow:yyyy-MM-dd}").FontSize(10);
                    column.Item().Text($"Status: {invoice.Status}").FontSize(10).FontColor(Colors.Green.Medium);
                });
            });
        }

        private void ComposeContent(IContainer container, Invoice invoice)
        {
            container.PaddingVertical(20).Column(column =>
            {
                column.Spacing(15);

                // Invoice Information Section
                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("BILL TO:").Bold().FontSize(12);
                        col.Item().PaddingTop(5).Text(invoice.Customer.Name).FontSize(11);
                        col.Item().Text(invoice.Customer.Email).FontSize(10);
                        col.Item().Text(invoice.Customer.Phone).FontSize(10);
                        if (invoice.Customer.Address != null)
                        {
                            col.Item().Text($"{invoice.Customer.Address.Street}").FontSize(10);
                            col.Item().Text($"{invoice.Customer.Address.City}, {invoice.Customer.Address.PostalCode}").FontSize(10);
                            col.Item().Text(invoice.Customer.Address.Country).FontSize(10);
                        }
                    });

                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignRight().Text($"Issue Date: {invoice.IssueDate:yyyy-MM-dd}").FontSize(11);
                        col.Item().AlignRight().Text($"Due Date: {invoice.DueDate:yyyy-MM-dd}").FontSize(11);
                        col.Item().AlignRight().PaddingTop(10).Text($"Total Amount").FontSize(11).Bold();
                        col.Item().AlignRight().Text($"${invoice.TotalAmount:N2}").FontSize(18).Bold().FontColor(Colors.Green.Darken2);
                    });
                });

                column.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                // Invoice Items Table
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3); // Description
                        columns.RelativeColumn(1); // Quantity
                        columns.RelativeColumn(1); // Unit Price
                        columns.RelativeColumn(1); // Tax %
                        columns.RelativeColumn(1); // Total
                    });

                    // Header
                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Blue.Lighten3).Padding(5).Text("Description").Bold();
                        header.Cell().Background(Colors.Blue.Lighten3).Padding(5).AlignCenter().Text("Qty").Bold();
                        header.Cell().Background(Colors.Blue.Lighten3).Padding(5).AlignRight().Text("Unit Price").Bold();
                        header.Cell().Background(Colors.Blue.Lighten3).Padding(5).AlignRight().Text("Tax %").Bold();
                        header.Cell().Background(Colors.Blue.Lighten3).Padding(5).AlignRight().Text("Total").Bold();
                    });

                    // Items
                    foreach (var item in invoice.Items)
                    {
                        table.Cell().Padding(5).Text(item.Description);
                        table.Cell().Padding(5).AlignCenter().Text(item.Quantity.ToString());
                        table.Cell().Padding(5).AlignRight().Text($"${item.UnitPrice:N2}");
                        table.Cell().Padding(5).AlignRight().Text($"{item.TaxRate}%");
                        table.Cell().Padding(5).AlignRight().Text($"${item.LineTotal:N2}");
                    }
                });

                column.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                // Totals Section
                column.Item().AlignRight().Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        row.AutoItem().PaddingRight(20).Text("Subtotal:").FontSize(11);
                        row.AutoItem().Text($"${invoice.SubTotal:N2}").FontSize(11);
                    });
                    col.Item().Row(row =>
                    {
                        row.AutoItem().PaddingRight(20).Text("Tax:").FontSize(11);
                        row.AutoItem().Text($"${invoice.TaxAmount:N2}").FontSize(11);
                    });
                    col.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                    col.Item().PaddingTop(5).Row(row =>
                    {
                        row.AutoItem().PaddingRight(20).Text("Total:").FontSize(14).Bold();
                        row.AutoItem().Text($"${invoice.TotalAmount:N2}").FontSize(14).Bold().FontColor(Colors.Blue.Darken2);
                    });
                });

                // Payment Information
                column.Item().PaddingTop(30).Column(col =>
                {
                    col.Item().Text("Payment Information").FontSize(12).Bold();
                    col.Item().PaddingTop(5).Text($"Please remit payment by {invoice.DueDate:MMMM dd, yyyy}").FontSize(10);
                    col.Item().Text("Thank you for your business!").FontSize(10);
                });
            });
        }
    }
}
