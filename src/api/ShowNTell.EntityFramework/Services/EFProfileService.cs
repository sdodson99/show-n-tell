using Microsoft.EntityFrameworkCore;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.EntityFramework.Services
{
    public class EFProfileService : IProfileService
    {
        private readonly IShowNTellDbContextFactory _contextFactory;

        public EFProfileService(IShowNTellDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<User> GetProfile(string username)
        {
            using (ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Users
                    .Include(u => u.ImagePosts)
                        .ThenInclude(p => p.Likes)
                    .Include(u => u.ImagePosts)
                        .ThenInclude(p => p.Comments)
                    .Include(u => u.Followers)
                        .ThenInclude(f => f.Follower)
                    .Include(u => u.Following)
                        .ThenInclude(f => f.User)
                    .FirstOrDefaultAsync(u => u.Username == username);
            }
        }

        public async Task<IEnumerable<ImagePost>> GetImagePosts(string username)
        {
            using (ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.ImagePosts
                    .Include(p => p.User)
                    .Include(p => p.Likes)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.User)
                    .Where(p => p.User.Username == username)
                    .ToListAsync();
            }
        }
    }
}
