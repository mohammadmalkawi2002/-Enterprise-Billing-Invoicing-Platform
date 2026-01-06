using BillingInvoicingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Infrastructure.Data.Config
{
    public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
    {
        public void Configure(EntityTypeBuilder<InvoiceItem> builder)
        {
            builder.HasKey(ii=>ii.Id);
            builder.Property(ii=>ii.Description).HasMaxLength(500).IsRequired();
            builder.Property(ii => ii.Quantity)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(ii => ii.UnitPrice)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(ii => ii.TaxRate)
                .IsRequired()
                .HasPrecision(5, 2); //  0.00 - 100.00 %


            // LineTotal is calculated, ignore mapping
            builder.Ignore(ii => ii.LineTotal);
            builder.Ignore(ii => ii.LineSubTotal);
            builder.Ignore(ii => ii.LineTax);



            builder.Ignore(x => x.IsDeleted);  
            builder.Ignore(x => x.DeletedAt);

            builder.HasQueryFilter(x => !x.Invoice.IsDeleted);



        }
    }
}
