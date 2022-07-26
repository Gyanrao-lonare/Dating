using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
namespace API.Middelware
{
    public class ExeptionMiddelware
    {
        public RequestDelegate _Next { get; }
        private readonly ILogger<ExeptionMiddelware> _logger;
        public IHostEnvironment _env { get; }
        public ExeptionMiddelware(RequestDelegate next, ILogger<ExeptionMiddelware> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _Next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _Next(context);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                ? new ApiExceptions(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ApiExceptions(context.Response.StatusCode, "Internal Server Error");
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }

        }
    }

}