using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace BillingInvoicingPlatform.API.HangfireFilter
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        

        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            // Allow access only to authenticated users with Admin role
            return httpContext.User.Identity?.IsAuthenticated == true &&
                  httpContext.User.IsInRole("Admin");
          
        }

      
    }
}
