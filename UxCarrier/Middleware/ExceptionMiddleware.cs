using Microsoft.AspNetCore.Mvc;
using System.Net;
using UxCarrier.Models;

namespace UxCarrier.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            //custom exception example
            //catch (AccessViolationException avEx)
            //{
            //    _logger.LogError($"A new violation exception has been thrown: {avEx}");
            //    await HandleExceptionAsync(httpContext, avEx);
            //}
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                await HandleExceptionAsync(httpContext);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //context.Response.Redirect("/Home/Error");

            //context.Response.ContentType = "text/html";
            //await context.Response.WriteAsync("<html><body>");
            //await context.Response.WriteAsync("An remote error has occured: " + (HttpStatusCode)context.Response.StatusCode + context.TraceIdentifier + "<br>");
            //await context.Response.WriteAsync("<a href=\"/\">Home</a>");
            //await context.Response.WriteAsync("</body></html>");

            await context.Response.WriteAsync(new ApiResponse()
            {
                IsSuccess = false,
                StatusCode = (HttpStatusCode)context.Response.StatusCode,
                ErrorMessages = new List<string>() { context.TraceIdentifier }
            }.ToString());

        }
    }
}
