using BillingInvoicingPlatform.Domain.Entities;
using BillingInvoicingPlatform.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Infrastructure.Data.Config
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices");

            builder.HasKey(i=>i.Id);


            builder.Property(i => i.InvoiceNumber)
             .IsRequired()
             .HasMaxLength(20);

            builder.HasIndex(i => i.InvoiceNumber)
               .IsUnique();

            builder.Property(i => i.Status).HasConversion<int>();
                
             

            builder.Property(i => i.IssueDate)
              .IsRequired();

            builder.Property(i => i.DueDate)
                   .IsRequired();


            builder.Property(i => i.SubTotal)
               .HasPrecision(18, 2)
               .IsRequired();

            builder.Property(i => i.TaxAmount)
                   .HasPrecision(18, 2)
                   .IsRequired();

            // TotalAmount is calculated ==> not mapped
            builder.Ignore(i => i.TotalAmount);
            builder.Ignore(i => i.RemainingBalance);


            //Config Invoice ==> InvoiceItem Relationship:

            builder.HasMany(i=>i.Items)
                .WithOne(invItem=>invItem.Invoice)
                .HasForeignKey(invItem=>invItem.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);


            // Payment => Invoice relationship
            builder.HasMany(i=>i.Payments)
                .WithOne(invItem=>invItem.Invoice) 
                .HasForeignKey(invItem=>invItem.InvoiceId) 
                .OnDelete(DeleteBehavior.Restrict);
           


            //Query Filter(Soft Delete)
            builder.HasQueryFilter(i => !i.IsDeleted);

        }
    }
}
