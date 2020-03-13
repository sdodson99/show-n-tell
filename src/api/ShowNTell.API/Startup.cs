using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SeanDodson.GoogleJWTAuthentication.Extensions;
using ShowNTell.API.Models.Requests;
using ShowNTell.AzureStorage.Services;
using ShowNTell.AzureStorage.Services.BlobClientFactories;
using ShowNTell.Domain.Services;
using ShowNTell.Domain.Services.ImageSavers;
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
            services.AddAuthorization();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Show 'N Tell API", Version = "v1" });
            });

            services.AddSingleton<IImagePostService, EFImagePostService>();
            services.AddSingleton<IRandomImagePostService, EFRandomImagePostService>();
            services.AddSingleton<IImageSaver>(GetImageSaver());
            services.AddSingleton<AdminDataSeeder>();

            Action<DbContextOptionsBuilder> dbContextOptionsBuilderAction = GetDbContextOptionsBuilderAction();
            services.AddSingleton<IShowNTellDbContextFactory>(new ShowNTellDbContextFactory(dbContextOptionsBuilderAction));
            services.AddDbContext<ShowNTellDbContext>(dbContextOptionsBuilderAction);
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
            });

            app.UseStaticFiles("/" + STATIC_FILE_BASE_URI);

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();

            //Add cors for all alternative domains.
            app.UseCors(policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private Action<DbContextOptionsBuilder> GetDbContextOptionsBuilderAction()
        {
            string connectionString = Configuration.GetConnectionString("local-database");
            return o => o.UseSqlServer(connectionString);
        }

        private IImageSaver GetImageSaver()
        {
            IImageSaver imageSaver;

            if(Environment.IsProduction())
            {
                string connectionString = Configuration.GetConnectionString("blob-storage");

                imageSaver = new AzureBlobImageSaver(new AzureBlobClientFactory(connectionString, "images"));
            }
            else
            {
                string imageOutputPath = Path.Combine(Environment.WebRootPath, IMAGE_DIRECTORY_NAME);
                string baseUrl = Configuration.GetValue<string>("BaseUrl");
                string imageBaseUri = Path.Combine(baseUrl, STATIC_FILE_BASE_URI, IMAGE_DIRECTORY_NAME);

                imageSaver = new LocalImageSaver(imageOutputPath, imageBaseUri);
            }

            return imageSaver;
        }
    }
}
