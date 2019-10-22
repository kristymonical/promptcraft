using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SVT.Platform.Data;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _environment;

    public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environment)
    {
        _next = next;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext httpContext, SVTContext context)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            var user = await context.Users
                .Where(u => u.Name == httpContext.User.Identity.Name)
                .FirstOrDefaultAsync();

            if (user == null && _environment.IsDevelopment())
            {
                // @TODO: assign 'DevUser' to user
            }
            // @TODO: invoke Commands.Log.CreateLog...
            // @TODO: httpContext...path to trackingId header
            // @TODO: parameters: serialize '{ message, stack, user.Name, ...}', action: 'Queue Delivery', trackingId
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var serialized = JsonConvert.SerializeObject(new
        {
            Success = false,
            Message = exception.Message
        });

        return context.Response.WriteAsync(serialized);
    }
}