using System.Collections.Generic;
using System.Threading.Tasks;
using ShowNTell.Domain.Models;

namespace ShowNTell.Domain.Services
{
    public interface IFeedService
    {
         Task<IEnumerable<ImagePost>> GetFeed(string userEmail);
    }
}