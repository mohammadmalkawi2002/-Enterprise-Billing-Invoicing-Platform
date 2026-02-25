using BillingInvoicingPlatform.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Infrastructure.Identity.Seed
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Accountant.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
        }



        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {

            if (await userManager.FindByEmailAsync("invoicebillingplatform@gmail.com") is null)
            {
                var adminUser = new ApplicationUser
                {
                    FirstName = "Mohammad",
                    LastName = "Malkawi",
                    UserName = "admin",
                    Email = "invoicebillingplatform@gmail.com",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                   
                };

                var result=await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded) 
                { 
                await userManager.AddToRoleAsync(adminUser,Roles.Admin.ToString());
                }
            }
        }
    }
}
