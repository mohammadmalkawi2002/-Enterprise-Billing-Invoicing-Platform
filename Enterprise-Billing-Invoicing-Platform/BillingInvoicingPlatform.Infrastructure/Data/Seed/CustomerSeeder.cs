using BillingInvoicingPlatform.Domain.Entities;
using BillingInvoicingPlatform.Domain.Enums;
using BillingInvoicingPlatform.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Infrastructure.Data.Seed
{
    public static class CustomerSeeder
    {

        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (context.Customers.Any())
                return;

            var arabicNames = new[]
            {
            "شركة النور", "مؤسسة المستقبل", "الرواد للتجارة",
            "شركة الأمل", "الشرق الأوسط", "المتحدة للخدمات",
            "تقنية العرب", "الصفوة", "الازدهار", "الوفاق"
        };

            var englishNames = new[]
            {
            "Tech Solutions", "Future Corp", "Global Systems",
            "Smart Business", "Blue Ocean", "NextGen Ltd",
            "Digital World", "Alpha Group", "Vision Co", "Prime Services"
        };

            var locations = new[]
            {
            new { Country = "Jordan", City = "Amman" },
            new { Country = "Jordan", City = "Irbid" },
            new { Country = "UAE", City = "Dubai" },
            new { Country = "Saudi Arabia", City = "Riyadh" },
            new { Country = "Egypt", City = "Cairo" }
        };

            var customers = new List<Customer>();

            for (int i = 1; i <= 100; i++)
            {
                bool isArabic = i % 2 == 0;
                var name = isArabic
                    ? $"{arabicNames[i % arabicNames.Length]} {i}"
                    : $"{englishNames[i % englishNames.Length]} {i}";

                var location = locations[i % locations.Length];

                // ===== Status Logic =====
                CustomerStatus status = (i % 5 == 0) ? CustomerStatus.InActive : CustomerStatus.Active;

                customers.Add(new Customer
                {
                    Name = name,
                    Email = $"customer{i}@example.com",
                    Phone = $"+96279{i:D6}",
                    Status = status,
                    Address = new Address
                    {
                        Country = location.Country,
                        City = location.City,
                        Street = $"Street {i}",
                        PostalCode = "11118"
                    },
                    CreatedAt = DateTime.UtcNow.AddDays(-i)
                });
            }

            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }
    
    }
}
