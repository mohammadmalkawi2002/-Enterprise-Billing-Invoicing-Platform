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
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p=>p.Id);
            builder.Property(p => p.PaymentAmount)
                 .HasPrecision(18, 2)
                 .IsRequired();

            builder.Property(p => p.PaymentDate)
                .IsRequired();

            builder.Property(p => p.PaymentMethod)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(p => p.Note)
                .HasMaxLength(500)
                .IsRequired(false);


            // Soft delete query filter
            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
