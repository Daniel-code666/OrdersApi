namespace Orders.Api.Middleware
{
    public sealed class ErrorResponse
    {
        public int Code { get; set; }
        public string Error { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
