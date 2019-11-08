using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SVT.Platform.Commands;
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

    public async Task InvokeAsync(HttpContext httpContext, SVTContext svtContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            ContextCommands.RollbackChanges(svtContext);

            var userName = httpContext.User.Identity.Name;

            if (userName == null && _environment.EnvironmentName == "Development")
            {
                userName = "DevAPI";
            }

            ex.Data.Add("Application", _environment.ApplicationName);
            ex.Data.Add("Environment", _environment.EnvironmentName);

            await LogCommands.CreateLog<BaseErrorLog>(svtContext, new DataToLog<BaseErrorLog>
            {
                TrackingId = httpContext.Request.Headers["trackingId"],
                Data = new BaseErrorLog
                {
                    User = userName,
                    Message = "Unhandled Exception",
                    Error = new ErrorLog
                    {
                        Message = ex.Message,
                        StackTrace = ex.StackTrace,
                        Source = ex.Source,
                        Data = ex.Data
                    }
                }
            });
            await svtContext.SaveChangesAsync();

            // pass trackingId back to response so client can use it for their own logging
            httpContext.Response.Headers["trackingId"] = httpContext.Request.Headers["trackingId"];

            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var serialized = JsonConvert.SerializeObject(new
        {
            success = false,
            message = exception.Message
        });

        return context.Response.WriteAsync(serialized);
    }
}