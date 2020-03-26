using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;

namespace ShowNTell.EntityFramework.Services
{
    public class EFFeedService : IFeedService
    {
        private readonly IShowNTellDbContextFactory _contextFactory;

        public EFFeedService(IShowNTellDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<ImagePost>> GetFeed(string userEmail)
        {
            using(ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Users
                    .Include(u => u.Following)
                        .ThenInclude(f => f.User)
                            .ThenInclude(u => u.ImagePosts)
                                .ThenInclude(p => p.Likes)
                                    .ThenInclude(c => c.User)
                    .Include(u => u.Following)
                            .ThenInclude(f => f.User)
                                .ThenInclude(u => u.ImagePosts)
                                    .ThenInclude(p => p.Comments)
                                        .ThenInclude(c => c.User)
                    .Where(u => u.Email == userEmail)
                    .SelectMany(u => u.Following.SelectMany(f => f.User.ImagePosts))
                    .OrderByDescending(p => p.DateCreated)
                    .ToListAsync();
            }
        }
    }
}