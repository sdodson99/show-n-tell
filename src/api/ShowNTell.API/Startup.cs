using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SeanDodson.GoogleJWTAuthentication.Extensions;
using ShowNTell.API.Models.MappingProfiles;
using ShowNTell.API.Services.CurrentUsers;
using ShowNTell.API.Services.EventGridImageBlobDeletes;
using ShowNTell.API.Services.EventGridValidations;
using ShowNTell.API.Services.ImageOptimizations;
using ShowNTell.AzureStorage.Services;
using ShowNTell.AzureStorage.Services.BlobClientFactories;
using ShowNTell.Domain.Services;
using ShowNTell.Domain.Services.ImageStorages;
using ShowNTell.EntityFramework.DataSeeders;
using ShowNTell.EntityFramework.Services;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;

namespace ShowNTell.API
{
    public class Startup
    {
        private const string STATIC_FILE_BASE_URI = "static";
        private const string IMAGE_DIRECTORY_NAME = "images";

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Environment = environment;
            Configuration = CreateShowNTellConfiguration(configuration);
        }

        public ShowNTellConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddGoogleJWTAuthentication();
            services.AddAuthorization(o =>
            {
                o.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });
            services.AddCors();

            services.AddSwaggerGen(c =>
            {
                // Add API background information.
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Show 'N Tell API", 
                    Version = "v1",
                    Description = "The API for the Show 'N Tell social media platform."
                });

                // Add bearer token definition.
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
                { 
                    In = ParameterLocation.Header,
                    Description = "Please enter 'Bearer' following by a space and the JWT", 
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement 
                {
                    { 
                        new OpenApiSecurityScheme 
                        { 
                            Reference = new OpenApiReference 
                            { 
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" 
                            } 
                        },
                        new string[] { } 
                    } 
                });

                // Add documentation from C# XML.
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddSingleton<IUserService, EFUserService>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<IProfileService, EFProfileService>();
            services.AddSingleton<IFeedService, EFFeedService>();
            services.AddSingleton<IFollowService, EFFollowService>();
            services.AddSingleton<ILikeService, EFLikeService>();
            services.AddSingleton<ICommentService, EFCommentService>();
            services.AddSingleton<IImagePostService, EFImagePostService>();
            services.AddSingleton<ISearchService, EFSearchService>();
            services.AddSingleton<IRandomImagePostService, EFRandomImagePostService>();
            services.AddSingleton<IImageOptimizationService, NoneImageOptimizationService>();
            services.AddSingleton<IImageStorage>(CreateImageStorage());
            services.AddSingleton<AdminDataSeeder>();
            services.AddSingleton<IMapper>(new MapperFactory().CreateMapper());
            services.AddSingleton<IEventGridValidationService, EventGridValidationService>();
            services.AddSingleton<IEventGridImageBlobDeleteService>((c) => 
                new EventGridImageBlobDeleteService(c.GetRequiredService<IImagePostService>(), Configuration.ImageBlobDeleteToken));
            services.AddSingleton<IShowNTellDbContextFactory>(CreateShowNTellDbContextFactory());

            if(Environment.IsProduction())
            {
                services.AddLogging(options => {
                    options.AddApplicationInsights(Configuration.ApplicationInsightsKey);
                });
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Show 'N Tell API v1");
                c.RoutePrefix = string.Empty;
            });
            app.UseStaticFiles("/" + STATIC_FILE_BASE_URI);
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private IShowNTellDbContextFactory CreateShowNTellDbContextFactory()
        {
            return new ShowNTellDbContextFactory(o => o.UseSqlServer(Configuration.DatabaseConnectionString));
        }

        private IImageStorage CreateImageStorage()
        {
            IImageStorage storage; 

            if(Environment.IsProduction())
            {
                storage = new AzureBlobImageStorage(new AzureBlobClientFactory(Configuration.BlobStorageConnectionString, "images"));
            } 
            else
            {
                string imageOutputPath = Path.Combine(Environment.WebRootPath, IMAGE_DIRECTORY_NAME);
                string imageBaseUri = Path.Combine(Configuration.BaseUrl, STATIC_FILE_BASE_URI, IMAGE_DIRECTORY_NAME);

                storage = new LocalImageStorage(imageOutputPath, imageBaseUri);
            }

            return storage;
        }

        private ShowNTellConfiguration CreateShowNTellConfiguration(IConfiguration configuration)
        {
            ShowNTellConfiguration showNTellConfiguration = new ShowNTellConfiguration();
            if(Environment.IsProduction())
            {
                string keyVaultName = GetConfigurationValue(configuration, "KEY_VAULT_NAME");
                string keyVaultUri = $"https://{keyVaultName}.vault.azure.net";
                SecretClient keyVaultClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());

                showNTellConfiguration.DatabaseConnectionString = keyVaultClient.GetSecret("DATABASE-CONNECTION-STRING").Value.Value;
                showNTellConfiguration.ApplicationInsightsKey = keyVaultClient.GetSecret("APPLICATION-INSIGHTS-KEY").Value.Value;
                showNTellConfiguration.BlobStorageConnectionString = keyVaultClient.GetSecret("BLOB-STORAGE-CONNECTION-STRING").Value.Value;
            }
            else
            {
                showNTellConfiguration.DatabaseConnectionString = GetConfigurationValue(configuration, "DATABASE");
            }

            showNTellConfiguration.BaseUrl = GetConfigurationValue(configuration, "BASE_URL");
            showNTellConfiguration.ImageBlobDeleteToken = GetConfigurationValue(configuration, "IMAGE_BLOB_DELETE_TOKEN");

            return showNTellConfiguration;
        }

        private string GetConfigurationValue(IConfiguration configuration, string key)
        {
            string value = configuration.GetValue<string>(key);

            if(string.IsNullOrEmpty(value))
            {
                value = System.Environment.GetEnvironmentVariable(key);
            }

            return value;
        }

        public class ShowNTellConfiguration
        {
            public string DatabaseConnectionString { get; set; }
            public string ApplicationInsightsKey { get; set; }
            public string BlobStorageConnectionString { get; set; }
            public string BaseUrl { get; set; }
            public string ImageBlobDeleteToken { get; set; }
        }
    }
}
