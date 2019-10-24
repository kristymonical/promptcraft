using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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
            RollbackChanges(svtContext);

            var user = await svtContext.Users
                .Where(u => u.Name == httpContext.User.Identity.Name)
                .FirstOrDefaultAsync();

            if (user == null && _environment.IsDevelopment())
            {
                user = await svtContext.Users
                    .Where(u => u.Name == "DevAPI")
                    .FirstOrDefaultAsync();
            }

            ex.Data.Add("Application", _environment.ApplicationName);
            ex.Data.Add("Environment", _environment.EnvironmentName);

            await LogCommands.CreateLog<BaseErrorLog>(svtContext, new DataToLog<BaseErrorLog>
            {
                TrackingId = httpContext.Request.Headers["trackingId"],
                Data = new BaseErrorLog
                {
                    User = user.Name,
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

    private void RollbackChanges(SVTContext svtContext)
    {
        foreach (var entry in svtContext.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
            }
        }
    }
}