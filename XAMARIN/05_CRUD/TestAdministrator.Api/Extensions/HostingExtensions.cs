﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestAdministrator.Api.Services;

namespace TestAdministrator.Api.Extensions
{
    public static class HostingExtensions
    {
        /// <summary>
        /// Setzt die Einstellungen für die JWT Authentifizierung und setzt den UserService für
        /// die Dependency Injection im Konstruktor.
        /// Vgl. https://jasonwatmore.com/post/2019/10/11/aspnet-core-3-jwt-authentication-tutorial-with-example-api
        /// </summary>
        /// <param name="services">Von ASP.NET gesetzte Servicecollection.</param>
        /// <param name="secret">Secret zum Signieren der JWT aus appconfig.json</param>
        public static void ConfigureJwt(this IServiceCollection services, string secret)
        {
            byte[] key = Convert.FromBase64String(secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Damit der Token auch als GET Parameter in der Form ...?token=xxxx übergben
                // werden kann, reagieren wir auf den Event für ankommende Anfragen.
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = ctx =>
                    {
                        string token = ctx.Request.Query["token"];
                        if (!string.IsNullOrEmpty(token))
                            ctx.Token = ctx.Request.Query["token"];
                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Dependency Injection für das Userservice im Konstruktor von UserController
            services.AddScoped<UserService>();
        }

    }
}
