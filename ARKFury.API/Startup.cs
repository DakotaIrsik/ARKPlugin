using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Serilog;
using System;
using ArkFury.API.Extensions;
using ArkFury.Common;
using ArkFury.Common.Constants;
using ArkFury.Common.Extensions;
using ArkFury.Common.Interfaces;
using ArkFury.CommonNetCore;
using ArkFury.CommonWeb.Extensions;
using ArkFury.CommonWeb.Hubs;
using ArkFury.Core.Mapping;
using ArkFury.Entities.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System.IO;

namespace ArkFury.API
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        private AppSettings Settings => Configuration.Get<AppSettings>();
        public IWebHostEnvironment HostingEnvironment { get; }
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env?.ContentRootPath)
                    .AddEnvironmentVariables()
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            File.WriteAllText($"appsettings.{env.EnvironmentName}", "Sanity check for environment. No use.");
            Configuration = builder.Build();
            HostingEnvironment = env;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddHttpContext();
            services.AddAppSettings(Configuration);
            services.AddCrossOriginPolicy(CrossOrigins.Policies.Loose, Settings);
            services.AddLogging(Configuration);
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddRefitClient<IGeoDbAPI>().ConfigureHttpClient(c => c.BaseAddress = new Uri(Settings.ConnectionStrings.GeoDb));
            services.AddRefitClient<IOpenArkFuryAPI>().ConfigureHttpClient(c => c.BaseAddress = new Uri(Settings.ConnectionStrings.OpenArkFury));
            services.AddElasticSearch();
            services.AddRCON();

            services.AddDbContextPool<ArkShopContext>(options => options
              .UseMySql(Settings.ConnectionStrings.ArkShopMySql, mySqlOptions => mySqlOptions
                  .MigrationsAssembly("ArkFury.API")
                  .ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql))));

            services.AddBusinessLogic();
            services.AddSwagger(Configuration.Get<AppSettings>());
            services.AddNoCachingService();
            services.AddControllers()
                    .AddNewtonsoftJson();
        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Settings.Suite}.{Settings.Name} - {Settings.Environment} {Settings.Version}");
                c.RoutePrefix = string.Empty;
            });
            app.UseDeveloperExceptionPage();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseAuthentication();
            app.UseSerilogRequestLogging();
            app.UseRouting();
            //WARNING With endpoint routing, the CORS middleware must be configured to execute between the calls to UseRouting 
            //and UseEndpoints. Incorrect configuration will cause the middleware to stop functioning correctly.
            //https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-3.0
            app.UseCors(CrossOrigins.Policies.Loose);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}
