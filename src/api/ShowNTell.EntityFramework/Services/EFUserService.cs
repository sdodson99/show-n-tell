using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;

namespace ShowNTell.EntityFramework.Services
{
    public class EFUserService : IUserService
    {
        private readonly IShowNTellDbContextFactory _contextFactory;

        public EFUserService(IShowNTellDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<User> GetByEmail(string email)
        {
            using(ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Users
                    .Include(u => u.Following)
                        .ThenInclude(f => f.User)
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
        }

        public async Task<User> Create(User user)
        {
            using(ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    context.Users.Add(user);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message, "email", ex);
                }

                return user;
            }
        }
    }
}