namespace ECommmerce.Api.Responses
{
    public class ApiErrorResponse
    {
        public int Statuscode { get; set; }
        public string Message { get; set; } = string.Empty;

        public string? Details { get; set; }
    }
}
