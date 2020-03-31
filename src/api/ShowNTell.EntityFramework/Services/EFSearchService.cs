using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;

namespace ShowNTell.EntityFramework.Services
{
    public class EFSearchService : ISearchService
    {
        private readonly IShowNTellDbContextFactory _contextFactory;

        public EFSearchService(IShowNTellDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<ImagePost>> SearchImagePosts(string query)
        {
            using(ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.ImagePosts
                    .Include(p => p.User)
                    .Include(p => p.Likes)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.User)
                    .Include(p => p.Tags)
                        .ThenInclude(t => t.Tag)
                    .Where(p => p.UserEmail.Contains(query) ||
                        p.Description.Contains(query) ||
                        p.Tags.Select(t => t.Tag.Content).Any(t => t.Contains(query)))
                    .ToListAsync();
            }
        }
    }
}