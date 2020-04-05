using System.Threading.Tasks;
using ShowNTell.Domain.Models;

namespace ShowNTell.Domain.Services
{
    /// <summary>
    /// A service for liking and unliking image posts.
    /// </summary>
    public interface ILikeService
    {
        /// <summary>
        /// Like an image post.
        /// </summary>
        /// <param name="imagePostId">The id of the image post to like.</param>
        /// <param name="userEmail">The email of the user liking the image post.</param>
        /// <returns>The new like.</returns>
        /// <exception cref="ShowNTell.Domain.Exceptions.DuplicateLikeException">Thrown if user is attempting to like a post they already like.</exception>
        /// <exception cref="ShowNTell.Domain.Exceptions.OwnImagePostLikeException">Thrown if user is attempting to like their own post.</exception>
        /// <exception cref="ShowNTell.Domain.Exceptions.EntityNotFoundException<int>">Thrown if the image post to like does not exist.</exception>
        /// <exception cref="ShowNTell.Domain.Exceptions.EntityNotFoundException<string>">Thrown if the user liking the image post does not exist.</exception>
        /// <exception cref="System.Exception">Thrown if creating the like fails.</exception>
        Task<Like> LikeImagePost(int imagePostId, string userEmail);

        /// <summary>
        /// Unlike an image post.
        /// </summary>
        /// <param name="imagePostId">The id of the image post to like.</param>
        /// <param name="userEmail">The email of the user liking the image post.</param>
        /// <returns>True/false for success.</returns>
        Task<bool> UnlikeImagePost(int imagePostId, string userEmail);
    }
}