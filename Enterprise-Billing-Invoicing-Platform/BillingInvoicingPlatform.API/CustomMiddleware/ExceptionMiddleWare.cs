using BillingInvoicingPlatform.Application.Exceptions;
using System.Net;
using System.Text.Json;
using System.Linq;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace BillingInvoicingPlatform.API.CustomMiddleware
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleWare> _logger;

        public ExceptionMiddleWare(RequestDelegate next,ILogger<ExceptionMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            var response = httpContext.Response;

            // If the response has already started, we cannot modify it here.
            if (response.HasStarted)
            {
                _logger.LogWarning("The response has already started, the exception middleware will not write the response body.");
                return;
            }

            response.ContentType = "application/json";

            var errorResponse = new ErrorResponse 
            { 
                 TraceId= httpContext.TraceIdentifier
            };

            switch (ex) 
            {
                case NotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Title = "Resource Not Found";
                    errorResponse.StatusCode = response.StatusCode;
                    errorResponse.Detail = ex.Message;
                    break;

                case BusinessException:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    errorResponse.Title = "Business Rule Violation";
                    errorResponse.StatusCode = response.StatusCode;
                    errorResponse.Detail = ex.Message;
                    break;

                case ValidationException validationException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Title = "Validation Error";
                    errorResponse.StatusCode = response.StatusCode;
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
                    errorResponse.StatusCode = response.StatusCode;
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
