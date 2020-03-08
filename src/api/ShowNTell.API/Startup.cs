using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SeanDodson.GoogleJWTAuthentication.Extensions;
using ShowNTell.AzureStorage.Services;
using ShowNTell.AzureStorage.Services.BlobClientFactories;
using ShowNTell.Domain.Services;
using ShowNTell.EntityFramework;
using ShowNTell.EntityFramework.Services;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;

namespace ShowNTell.API
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddGoogleJWTAuthentication();
            services.AddSingleton<IImagePostService, EFImagePostService>();

            IImageSaver imageSaver = GetImageSaver();
            services.AddSingleton<IImageSaver>(imageSaver);

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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private Action<DbContextOptionsBuilder> GetDbContextOptionsBuilderAction()
        {
            string connectionString = Configuration.GetConnectionString("database");
            return o => o.UseSqlServer(connectionString);
        }

        private IImageSaver GetImageSaver()
        {
            string connectionString = Configuration.GetConnectionString("blob-storage");
            return new AzureBlobImageSaver(new AzureBlobClientFactory(connectionString, "images"));
        }
    }
}
