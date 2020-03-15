using System.Threading.Tasks;
using ShowNTell.Domain.Models;

namespace ShowNTell.Domain.Services
{
    public interface ILikeService
    {
         Task<Like> LikeImagePost(int imagePostId, string userEmail);
         Task<bool> UnlikeImagePost(int imagePostId, string userEmail);
    }
}