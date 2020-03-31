using System.Collections.Generic;
using System.Threading.Tasks;
using ShowNTell.Domain.Models;

namespace ShowNTell.Domain.Services
{
    public interface ISearchService
    {
         Task<IEnumerable<ImagePost>> SearchImagePosts(string query);
    }
}