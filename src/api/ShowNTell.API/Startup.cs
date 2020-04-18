using System;
using System.IO;
using System.Net;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SeanDodson.GoogleJWTAuthentication.Extensions;
using ShowNTell.API.Models.MappingProfiles;
using ShowNTell.API.Models.Requests;
using ShowNTell.API.Services.CurrentUsers;
using ShowNTell.AzureStorage.Services;
using ShowNTell.AzureStorage.Services.BlobClientFactories;
using ShowNTell.Domain.Services;
using ShowNTell.Domain.Services.ImageStorages;
using ShowNTell.EntityFramework;
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
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
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
            services.AddSingleton<IImageStorage>(GetImageStorage());
            services.AddSingleton<AdminDataSeeder>();

            Action<DbContextOptionsBuilder> dbContextOptionsBuilderAction = GetDbContextOptionsBuilderAction();
            services.AddSingleton<IShowNTellDbContextFactory>(new ShowNTellDbContextFactory(dbContextOptionsBuilderAction));
            services.AddDbContext<ShowNTellDbContext>(dbContextOptionsBuilderAction);

            IMapper mapper = new MapperFactory().CreateMapper();
            services.AddSingleton<IMapper>(mapper);

            services.AddLogging(options => {
                if(Environment.IsProduction())
                {
                    string instrumentationKey = GetConfigurationValue("APPLICATION_INSIGHTS_KEY");
                    options.AddApplicationInsights(instrumentationKey);
                }
            });

            if(Environment.IsProduction())
            {
                services.AddLetsEncrypt();
            }

            // services.Configure<KestrelServerOptions>(c => {
            //     c.ConfigureHttpsDefaults(h => {
            //         h.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2($"Certificates/{Environment.EnvironmentName}.pfx", GetConfigurationValue("HTTPS_PASSWORD"));
            //     });
            // });
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

            //Add cors for all alternative domains.
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

        private Action<DbContextOptionsBuilder> GetDbContextOptionsBuilderAction()
        {
            string connectionString = GetConfigurationValue("database");
            return o => o.UseSqlServer(connectionString);
        }

        private IImageStorage GetImageStorage()
        {
            IImageStorage imageStorage;

            if(Environment.IsProduction())
            {
                string connectionString = GetConfigurationValue("BLOB_STORAGE");

                imageStorage = new AzureBlobImageStorage(new AzureBlobClientFactory(connectionString, "images"));
            }
            else
            {
                string imageOutputPath = Path.Combine(Environment.WebRootPath, IMAGE_DIRECTORY_NAME);
                string baseUrl = GetConfigurationValue("BaseUrl");
                string imageBaseUri = Path.Combine(baseUrl, STATIC_FILE_BASE_URI, IMAGE_DIRECTORY_NAME);

                imageStorage = new LocalImageStorage(imageOutputPath, imageBaseUri);
            }

            return imageStorage;
        }

        private string GetConfigurationValue(string key)
        {
            string value = Configuration.GetValue<string>(key);

            if(string.IsNullOrEmpty(value))
            {
                value = System.Environment.GetEnvironmentVariable(key);
            }

            return value;
        }
    }
}
