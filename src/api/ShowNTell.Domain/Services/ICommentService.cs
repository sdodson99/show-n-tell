using System.Threading.Tasks;
using ShowNTell.Domain.Models;

namespace ShowNTell.Domain.Services
{
    /// <summary>
    /// A service for creating comments.
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// Create a new comment.
        /// </summary>
        /// <param name="comment">The comment to create.</param>
        /// <returns>The new created comment with an id.</returns>
        /// <exception cref="System.Exception">Thrown if creating the comment fails.</exception>
         Task<Comment> Create(Comment comment);
    }
}