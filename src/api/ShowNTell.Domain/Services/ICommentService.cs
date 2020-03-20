using System.Threading.Tasks;
using ShowNTell.Domain.Models;

namespace ShowNTell.Domain.Services
{
    public interface ICommentService
    {
         Task<Comment> Create(Comment comment);
    }
}