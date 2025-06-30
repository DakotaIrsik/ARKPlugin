using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ArkFury.Common;
using ArkFury.Common.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArkFury.CommonWeb.Extensions
{
    public static class ServiceCollectionWebExtensions
    {
        public static IServiceCollection AddMvc(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new ResponseCacheAttribute
                {
                    Duration = 0
                });

            });

            return services;
        }

        public static IServiceCollection AddCrossOriginPolicy(this IServiceCollection services, string policyName, AppSettings settings)
        {
            var origins = new List<string> { settings.Url };
            if (policyName == CrossOrigins.Policies.Loose)
            {
                origins.Add("http://localhost:8081");
                origins.Add("http://localhost:8080");
                origins.Add("http://localhost:4200");
                origins.Add("https://arkfury.com");
                origins.Add("https://shorelinegames.com");
                origins.Add("http://shorelinegames.com");
            }
            services.AddCors(options =>
            {
                options.AddPolicy(policyName,
                    builder =>
                    {
                        builder.WithOrigins(origins.ToArray())
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });
            return services;
        }
    }
}
