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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user=>user.Id);
            builder.Property(user => user.Username).HasMaxLength(255).IsRequired();
            builder.Property(user => user.EmailAddress).HasMaxLength(255).IsRequired();
            builder.Property(user => user.PhotoUrl).HasMaxLength(2048).IsRequired(false);
            builder.Property(user => user.Role).HasConversion<int>();



        }
    }
}
