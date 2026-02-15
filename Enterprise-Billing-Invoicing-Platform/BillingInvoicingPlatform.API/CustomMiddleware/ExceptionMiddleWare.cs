using BillingInvoicingPlatform.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace BillingInvoicingPlatform.API.CustomMiddleware
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logge;

        public ExceptionMiddleWare(RequestDelegate next,ILogger<ExceptionMiddleWare> logger)
        {
            _next = next;
            _logge = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logge.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            var response = httpContext.Response;
            response.ContentType="application/json";

            var errorResponse = new ErrorResponse 
            { 
                 TraceId= httpContext.TraceIdentifier
            };

            switch (ex) 
            {
                case NotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Title = "Resource Not Found";
                    errorResponse.StatusCode = 404;
                    errorResponse.Detail = ex.Message;
                    break;

                case BusinessException:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    errorResponse.Title = "Business Rule Violation";
                    errorResponse.StatusCode = 409;
                    errorResponse.Detail = ex.Message;
                    break;

                case FluentValidation.ValidationException validationException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Title = "Validation Error";
                    errorResponse.StatusCode = 400;
                    errorResponse.Detail = "One or more validation errors occurred.";
                    errorResponse.Errors = validationException.Errors
                       .GroupBy(e => e.PropertyName)
                       .ToDictionary(
                           g => g.Key,
                           g => g.Select(e => e.ErrorMessage).ToArray()
                       );
                       break;
                        
                    default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Title = "Internal Server Error";
                    errorResponse.StatusCode = 500;
                    errorResponse.Detail = "An unexpected error occurred.";
                    break;

            }

            var jsonResult= JsonSerializer.Serialize(errorResponse,new JsonSerializerOptions
            {
              PropertyNamingPolicy=JsonNamingPolicy.CamelCase,  
            });
               
            await response.WriteAsync(jsonResult);


        }
    }
}
