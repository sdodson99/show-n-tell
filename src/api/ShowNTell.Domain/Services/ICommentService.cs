using System.Threading.Tasks;
using ShowNTell.Domain.Models;

namespace ShowNTell.Domain.Services
{
    /// <summary>
    /// A service for performing CRUD on comments.
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

        /// <summary>
        /// Update the content of a comment.
        /// </summary>
        /// <param name="id">The id of the comment to update.</param>
        /// <param name="content">The new comment content.</param>
        /// <returns>The updated comment.</returns>
        /// <exception cref="ShowNTell.Domain.Exceptions.EntityNotFoundException<int>">Thrown if the comment to update does not exist.</exception>
        /// <exception cref="System.Exception">Thrown if updating the comment fails.</exception>
        Task<Comment> Update(int id, string content);

        /// <summary>
        /// Delete a comment on an image post.
        /// </summary>
        /// <param name="commentId">The id of the comment.</param>
        /// <returns>True/false for success.</returns>
         Task<bool> Delete(int commentId);

         /// <summary>
         /// Check if a user owns a comment.
         /// </summary>
         /// <param name="commentId">The id of the comment.</param>
         /// <param name="userEmail">The email of the user to check.</param>
         /// <returns>True/false for if the user owns the comment.</returns>
         Task<bool> IsCommentOwner(int commentId, string userEmail);

         /// <summary>
         /// Check if a user can delete a comment.
         /// </summary>
         /// <param name="commentId">The id of the comment.</param>
         /// <param name="userEmail">The email of the user to check.</param>
         /// <returns>True/false for can delete.</returns>
         Task<bool> CanDelete(int commentId, string userEmail);
    }
}