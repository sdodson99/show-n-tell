using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShowNTell.Domain.Models;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;

namespace ShowNTell.EntityFramework.DataSeeders
{
    public class AdminDataSeeder
    {
        private readonly IShowNTellDbContextFactory _contextFactory;

        public AdminDataSeeder(IShowNTellDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void Seed()
        {
            using(ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                context.Database.Migrate();

                if(context.Users.Count() == 0)
                {
                    context.Users.Add(new User()
                    {
                        Email = "admin@showntell.com",
                        Username = "admin"
                    });
                }

                context.SaveChanges();
            }
        }
    }
}