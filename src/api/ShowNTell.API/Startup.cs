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

            Action<DbContextOptionsBuilder> dbContextOptionsBuilderAction = GetDbContextOptionsBuilderAction();
            services.AddSingleton<IShowNTellDbContextFactory>(new ShowNTellDbContextFactory(dbContextOptionsBuilderAction));
            services.AddDbContext<ShowNTellDbContext>(dbContextOptionsBuilderAction);
        }

        private Action<DbContextOptionsBuilder> GetDbContextOptionsBuilderAction()
        {
            string connectionString = "";

            // Change data source for production.
            if(Environment.IsProduction())
            {
                connectionString = Configuration.GetConnectionString("azure-sql");
            }
            else
            {
                connectionString = Configuration.GetConnectionString("vs-local");
            }

            return o => o.UseSqlServer(connectionString);
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
    }
}
