using School_Management.Helpers;
using System.Net;
using System.Text.Json;

namespace School_Management.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware( RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.ContentType = "application/json";

                context.Response.StatusCode = 
                    (int)HttpStatusCode.InternalServerError;

                var response = new ApiResponse<object>
                {
                    Success = false,

                    Message = "Internal Server Error",

                    Data = null
                };

                var jsonResponse =
                    JsonSerializer.Serialize(response,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
