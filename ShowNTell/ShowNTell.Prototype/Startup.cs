using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShowNTell.Prototype.Extensions;
using ShowNTell.Prototype.Models;
using Microsoft.EntityFrameworkCore;
using SeanDodson.GoogleJWTAuthentication.Extensions;

namespace ShowNTell.Prototype
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddGoogleJWTAuthentication();
            services.AddAuthorization();
            services.AddDbContext<ShowNTellDbContext>(o => o.UseInMemoryDatabase("show-n-tell"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Allow uploaded files to be served.
            app.UseStaticFiles();

            //Serve static site from directory.
            app.UseClient("public");

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

            //Setup endpoints from controllers.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
