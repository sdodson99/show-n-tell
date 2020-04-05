using ShowNTell.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.Domain.Services
{
    /// <summary>
    /// A service for managing CRUD on image posts.
    /// </summary>
    public interface IImagePostService
    {
        /// <summary>
        /// Get an image post by id.
        /// </summary>
        /// <param name="id">The id of the image post to retrieve.</param>
        /// <returns>The image post represented by the id. Null if the image post does not exist.</returns>
        Task<ImagePost> GetById(int id);

        /// <summary>
        /// Create a new image post.
        /// </summary>
        /// <param name="imagePost">The image post to create.</param>
        /// <returns>The new created image post with an id.</returns>
        /// <exception cref="System.Exception">Thrown if creating the new image post fails.</exception>
        Task<ImagePost> Create(ImagePost imagePost);

        /// <summary>
        /// Update an image post with a new description.
        /// </summary>
        /// <param name="id">The id of the image post to update.</param>
        /// <param name="description">The updated description for the image post.</param>
        /// <returns>The updated image post.</returns>
        /// <exception cref="ShowNTell.Domain.Exceptions.EntityNotFoundException<int>">Thrown if the image post to update does not exist.</exception>
        /// <exception cref="System.Exception">Thrown if updating the image post fails.</exception>
        Task<ImagePost> Update(int id, string description);

        /// <summary>
        /// Update an image post with a new description and new tags.
        /// </summary>
        /// <param name="id">The id of the image post to update.</param>
        /// <param name="description">The updated description for the image post.</param>
        /// <param name="tags">The updated tags for the image post.</param>
        /// <returns>The updated image post.</returns>
        /// <exception cref="ShowNTell.Domain.Exceptions.EntityNotFoundException<int>">Thrown if the image post to update does not exist.</exception>
        /// <exception cref="System.Exception">Thrown if updating the image post fails.</exception>
        Task<ImagePost> Update(int id, string description, IEnumerable<Tag> tags);

        /// <summary>
        /// Delete an image post.
        /// </summary>
        /// <param name="id">The id of the image post to delete.</param>
        /// <returns>True/false for success.</returns>
        Task<bool> Delete(int id);

        /// <summary>
        /// Check if a user is the author an image post.
        /// </summary>
        /// <param name="id">The id of the image post.</param>
        /// <param name="email">The email of the user.</param>
        /// <returns>True/false for if the user is the image post's author.</returns>
        Task<bool> IsAuthor(int id, string email);
    }
}
