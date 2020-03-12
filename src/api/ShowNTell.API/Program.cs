using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShowNTell.EntityFramework;
using ShowNTell.EntityFramework.DataSeeders;

namespace ShowNTell.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            // Seed the database.
            using (IServiceScope scope = host.Services.CreateScope())
            {
                AdminDataSeeder seeder = scope.ServiceProvider.GetRequiredService<AdminDataSeeder>();
                seeder.Seed();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://localhost:5000/", "https://localhost:5001/");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
