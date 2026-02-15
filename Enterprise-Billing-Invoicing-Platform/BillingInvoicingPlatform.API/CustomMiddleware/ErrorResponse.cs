namespace BillingInvoicingPlatform.API.CustomMiddleware
{
    public class ErrorResponse
    {
        public string Title { get; set; }= string.Empty;
        public int StatusCode { get; set; }
        public string Detail { get; set; }= string.Empty;
        public string TraceId { get; set; }= string.Empty;

        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
