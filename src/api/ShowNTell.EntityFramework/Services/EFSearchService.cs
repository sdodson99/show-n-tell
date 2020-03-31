using System.Collections.Generic;
using System.Threading.Tasks;
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
            throw new System.NotImplementedException();
        }
    }
}