using BillingInvoicingPlatform.Domain.Entities;
using BillingInvoicingPlatform.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Infrastructure.Data.Config
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(customer => customer.Id);
            //Customer Name Required (2-100 characters)
             builder.Property(Customer => Customer.Name).HasMaxLength(100).IsRequired();
             builder.Property(customer => customer.Email).HasMaxLength(150).IsRequired();     
             builder.HasIndex(customer => customer.Email).IsUnique();
             builder.Property(customer => customer.Phone).HasMaxLength(20).IsRequired();


            //Config Value Conversion for Enums:


            builder.Property(customer => customer.Status).IsRequired().HasConversion<int>();

            //Config for OwnedType(address):
            builder.OwnsOne(customer => customer.Address, address => 
            {
                address.Property(p => p.Country).HasMaxLength(30).HasColumnName("Country");
                address.Property(p => p.City).HasMaxLength(100).HasColumnName("City");
                address.Property(p => p.Street).HasMaxLength(150).HasColumnName("Street");
                address.Property(p => p.PostalCode).HasMaxLength(5).HasColumnName("PostalCode");

            
            });


            //Config Customer-Invoice RelationShip:

            builder.HasMany(customer => customer.Invoices)
                 .WithOne(i => i.Customer)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);//can not delete Customer has invoice


            //QueryFilter()
            builder.HasQueryFilter(customer=>customer.Status !=CustomerStatus.Deleted);

            // Optional: prevent using IsDeleted column for Customer
            builder.Ignore(customer => customer.IsDeleted);
        }
    }
}
