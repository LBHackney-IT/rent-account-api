using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using RentAccountApi.V1.Gateways;
using RentAccountApi.V1.Infrastructure;
using RentAccountApi.V1.UseCase;
using RentAccountApi.V1.UseCase.Interfaces;
using RentAccountApi.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon;
using RentAccountApi.V1.Boundary;
using System.Globalization;
using Amazon.Runtime.Internal.Util;
using Gateways;
using Amazon.DynamoDBv2.Model;

namespace RentAccountApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private static List<ApiVersionDescription> _apiVersions { get; set; }
        private const string ApiName = "Rent Account API";

        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true; // assume that the caller wants the default version if they don't specify
                o.ApiVersionReader = new UrlSegmentApiVersionReader(); // read the version number from the url segment header)
            });

            services.AddSingleton<IApiVersionDescriptionProvider, DefaultApiVersionDescriptionProvider>();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Token",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Your Hackney API Key",
                        Name = "X-Api-Key",
                        Type = SecuritySchemeType.ApiKey
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Token" }
                        },
                        new List<string>()
                    }
                });

                //Looks at the APIVersionAttribute [ApiVersion("x")] on controllers and decides whether or not
                //to include it in that version of the swagger document
                //Controllers must have this [ApiVersion("x")] to be included in swagger documentation!!
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    apiDesc.TryGetMethodInfo(out var methodInfo);

                    var versions = methodInfo?
                        .DeclaringType?.GetCustomAttributes()
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions).ToList();

                    return versions?.Any(v => $"{v.GetFormattedApiVersion()}" == docName) ?? false;
                });

                //Get every ApiVersion attribute specified and create swagger docs for them
                foreach (var apiVersion in _apiVersions)
                {
                    var version = $"v{apiVersion.ApiVersion.ToString()}";
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        Title = $"{ApiName}-api {version}",
                        Version = version,
                        Description = $"{ApiName} version {version}. Please check older versions for depreciated endpoints."
                    });
                }

                c.CustomSchemaIds(x => x.FullName);
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath);
            });

            ConfigureDbContext(services);
            RegisterGateways(services);
            RegisterUseCases(services);
        }

        private static void ConfigureDbContext(IServiceCollection services)
        {

            //Dynamo DB
            var tableName = Environment.GetEnvironmentVariable("TABLE_NAME");
            LambdaLogger.Log($"Dynamo table name {tableName}");
            var dynamoConfig = new AmazonDynamoDBConfig { RegionEndpoint = RegionEndpoint.EUWest2 };
            var dynamoDbClient = new DynamoDBClient(dynamoConfig);
            // Set the endpoint URL

            var env = Environment.GetEnvironmentVariable("ENV");
            if (env.ToUpper(CultureInfo.CurrentCulture) == "local")
            {
                dynamoConfig.ServiceURL = "http://localhost:8000";
            }
            else
            {
                dynamoConfig.RegionEndpoint = RegionEndpoint.EUWest2;
            }
            LambdaLogger.Log(string.Format("ServiceURL-{0}, Region-{1}, Table-{2}", dynamoDbClient.Client.Config.DetermineServiceURL(), dynamoDbClient.Client.Config.RegionEndpoint, tableName));
            services.AddTransient<IDynamoDBHandler>(sp => new DynamoDBHandler(tableName, dynamoDbClient));
        }

        private static void RegisterGateways(IServiceCollection services)
        {
            //var tableName = Environment.GetEnvironmentVariable("TABLE_NAME");
            services.AddScoped<IAuditDatabaseGateway, AuditDatabaseGateway>();
            var crmUrl = Environment.GetEnvironmentVariable("CRM_API_ENDPOINT");

            services.AddHttpClient<ICRMGateway, CRMGateway>(a =>
            {
                a.BaseAddress = new Uri(crmUrl);
            });

            var crmTokenUrl = Environment.GetEnvironmentVariable("CRM_TOKEN_ENDPOINT");
            var crmTokenKey = Environment.GetEnvironmentVariable("CRM_TOKEN_KEY");

            services.AddHttpClient<ICRMTokenGateway, CRMTokenGateway>(a =>
            {
                a.BaseAddress = new Uri(crmTokenUrl);
                a.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", crmTokenKey);
            });
        }

        private static void RegisterUseCases(IServiceCollection services)
        {
            services.AddScoped<IPostAuditUseCase, PostAuditUseCase>();
            services.AddScoped<IGetAuditByUserUseCase, GetAuditByUserUseCase>();
            services.AddScoped<ICheckRentAccountExistsUseCase, CheckRentAccountExistsUseCase>();
            services.AddScoped<IGetRentAccountUseCase, GetRentAccountUseCase>();
            services.AddScoped<IGetLinkedAccountUseCase, GetLinkedAccountUseCase>();
            services.AddScoped<IDeleteLinkedAccountUseCase, DeleteLinkedAccountUseCase>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //Get All ApiVersions,
            var api = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
            _apiVersions = api.ApiVersionDescriptions.ToList();

            //Swagger ui to view the swagger.json file
            app.UseSwaggerUI(c =>
            {
                foreach (var apiVersionDescription in _apiVersions)
                {
                    //Create a swagger endpoint for each swagger version
                    c.SwaggerEndpoint($"{apiVersionDescription.GetFormattedApiVersion()}/swagger.json",
                        $"{ApiName}-api {apiVersionDescription.GetFormattedApiVersion()}");
                }
            });
            app.UseSwagger();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                // SwaggerGen won't find controllers that are routed via this technique.
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
