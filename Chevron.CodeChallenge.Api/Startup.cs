using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Chevron.CodeChallenge.Implementations;
using Chevron.CodeChallenge.Interfaces;
using Chevron.CodeChallenge.Persistance;
using Chevron.CodeChallenge.UnitOfWork;
using Chevron.CodeChallenge.Service.Implementations;
using Chevron.CodeChallenge.Service.Interfaces;
using System.Text;
using Chevron.CodeChallege.Api.Helpers;
using Chevron.CodeChallege.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Chevron.CodeChallenge.Api.Helpers;
using Chevron.CodeChallenge.Api.Middleware;

namespace Chevron.CodeChallege
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AppSettings.Secret = Configuration["AppSettings:Secret"];

            JwtSettings.Key = Configuration["jwtSettings:key"];
            JwtSettings.Issuer = Configuration["jwtSettings:issuer"];
            JwtSettings.Audience = Configuration["jwtSettings:audience"];
            JwtSettings.MinutesToExpiration = int.Parse(Configuration["jwtSettings:minutesToExpiration"]); 

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("JwtBearer", jwtBearerOptions =>
            {

                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Key)),

                    ValidateIssuer = true,
                    ValidIssuer = JwtSettings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = JwtSettings.Audience,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(JwtSettings.MinutesToExpiration),

                };
            });

            services.AddControllers()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(opt =>
                {
                    var serializerOptions = opt.JsonSerializerOptions;
                    serializerOptions.IgnoreNullValues = true;
                    serializerOptions.IgnoreReadOnlyProperties = false;
                    serializerOptions.WriteIndented = true;
                });

            // Configure the persistence in another layer
            MongoDatabasePersistance.Configure();

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Chevron Code Challenge",
                    Description = "Swagger surface",
                    Contact = new OpenApiContact
                    {
                        Name = "Tomas G Iglesias",
                        Email = "tomasguagnini.iglesias@gmail.com",
                        Url = new Uri("http://www.generic.net.br")
                    }
                });

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"},
                            Scheme = "oauth2",
                                Name = "Authorization",
                                In = ParameterLocation.Header
                        },
                        new string[] { }
                    }
                    });
            });

            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Repository Pattern and Unit of Work API v1.0");
            });

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseMiddleware<JwtMiddleware>();

            app.UseAuthentication();
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IMongoDatabaseContext, MongoDatabaseContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IBoxerRepository, BoxerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBoxerService, BoxerService>();
        }
    }
}
