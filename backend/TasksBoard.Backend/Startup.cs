using System.Collections.Generic;
using System.Reflection;

using AutoMapper;

using FluentValidation.AspNetCore;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using Serilog;

using TasksBoard.Backend.Infrastructure;
using TasksBoard.Backend.Infrastructure.Context;
using TasksBoard.Backend.Infrastructure.Errors;
using TasksBoard.Backend.Infrastructure.Initializers;
using TasksBoard.Backend.Infrastructure.Security;

namespace TasksBoard.Backend
{
    public class Startup
    {
        private readonly AppSettings _appSettings;

        public Startup(IConfiguration configuration, AppSettings.SourceType sourceType = AppSettings.SourceType.EnvironmentVariables)
        {
            _appSettings = new AppSettings(sourceType, configuration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Information("Database injector init started");
            services.AddScoped<IDbContextInjector, DbContextInjector>(s => new DbContextInjector(
                _appSettings.MasterDatabaseConnectionString,
                _appSettings.SyncReplicaDatabaseConnectionString
            ));

            DatabaseInitializer.Migrate(_appSettings.MasterDatabaseConnectionString);

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(DbContextTransactionPipelineBehavior<,>));

            services.AddLocalization(x => x.ResourcesPath = "Resources");

            // Inject an implementation of ISwaggerProvider with defaulted settings applied
            services.AddSwaggerGen(x =>
            {
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new string[] { }
                    }
                });
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "TasksBoard API", Version = "v1" });
                x.CustomSchemaIds(y => y.FullName);
                x.DocInclusionPredicate((version, apiDescription) => true);
                x.TagActionsBy(y => new List<string>
                {
                    y.GroupName
                });
            });

            services.AddCors();
            services.AddMvc(opt =>
            {
                opt.Conventions.Add(new GroupByApiRootConvention());
                opt.Filters.Add(typeof(ValidatorActionFilter));
                opt.EnableEndpointRouting = false;
            })
                .AddJsonOptions(opt => opt.JsonSerializerOptions.IgnoreNullValues = true)
                .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddAutoMapper(GetType().Assembly);

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();

            services.AddJwt();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseRequestLogging();
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseCors(builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            app.UseMvc();
            app.UseAuthentication();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });

            // Enable middleware to serve swagger-ui assets(HTML, JS, CSS etc.)
            app.UseSwaggerUI(x => { x.SwaggerEndpoint("/swagger/v1/swagger.json", "RealWorld API V1"); });
        }
    }
}