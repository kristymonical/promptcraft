using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SVT.Platform.Data;
using SVT.Platform.Filters;
using FluentValidation;
using FluentValidation.AspNetCore;
using SVT.Platform.Controllers;
using SVT.Platform.Validators;
using Microsoft.AspNetCore.Mvc;
using Aethon;
using System.Net.Http;
using System;

namespace SVT.Platform
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // All FE is handled by React. No need for views or pages.
            services.AddControllers();

            services
                .AddMvc(options =>
                {
                    options.Filters.Add(new ModelStateFilter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddFluentValidation();

            // Validators
            services.AddTransient<IValidator<AreaController.ByLocationName>, ByLocationNameValidator>();
            // services.AddTransient<IValidator<CartController.ByDeliveryType>, ByDeliveryTypeValidator>();
            services.AddTransient<IValidator<CartController.MoveCartRequest>, MoveCartRequestValidator>();
            services.AddTransient<IValidator<CartController.CleanInfoRequest>, CleanInfoRequestValidator>();
            services.AddTransient<IValidator<DeliveryController.DeliveryRequests>, DeliveryRequestsValidator>();
            services.AddTransient<IValidator<DeliveryQueueController.ByPoolRequest>, ByPoolRequestValidator>();
            services.AddTransient<IValidator<DeliveryQueueController.PriorityQuery>, PriorityQueryValidator>();
            services.AddTransient<IValidator<DeliveryQueueController.PriorityRoute>, PriorityRouteValidator>();
            services.AddTransient<IValidator<LogController.LogRequest<LogController.WebAppLogRequest>>, LogRequestValidator>();

            // DI
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<SVTContext>(options => options
                .UseLazyLoadingProxies()
                .UseSqlServer(Configuration.GetConnectionString("PlatformDb")));

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddSingleton<IConfiguration>(Configuration);

            var client = new HttpClient { BaseAddress = new Uri("http://localhost:3000/") };

            services.AddSingleton<AethonApi>(new AethonApi(client));

            services.AddSingleton<IWebHostEnvironment>(Environment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SVTContext context)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            // run migrations automatically AND use HSTS if we're not in dev mode
            if (!env.IsDevelopment())
            {
                context.Database.Migrate();
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
