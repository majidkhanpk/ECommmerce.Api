using System.Net;
using System.Text.Json;
using ECommmerce.Api.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ECommmerce.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public ExceptionHandlingMiddleware(RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        public async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Log the exception or perform any additional processing here if needed
            _logger.LogError(exception, "An unhandled exception occurred.");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // You can also customize the response based on the type of exception
            // For example, you could return different status codes for different exceptions
            var response = new ApiErrorResponse
            {
                Statuscode = context.Response.StatusCode,
                Message = "An unexpected error occurred. Please try again later.",
                Details = _environment.IsDevelopment() ? exception.ToString() : null
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }

    }
}
