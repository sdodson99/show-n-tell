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
    public class EFRandomImagePostService : IRandomImagePostService
    {
        private readonly IShowNTellDbContextFactory _contextFactory;

        public EFRandomImagePostService(IShowNTellDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<ImagePost> GetRandom()
        {
            using(ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.ImagePosts
                    .Include(p => p.User)
                    .Include(p => p.Likes)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.User)
                    .OrderBy(p => Guid.NewGuid())
                    .FirstOrDefaultAsync();
            }
        }
    }
}
