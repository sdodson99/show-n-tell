using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.Domain.Services.ImageStorages
{
    /// <summary>
    /// A service for saving and deleting image files.
    /// </summary>
    public interface IImageStorage
    {
        /// <summary>
        /// Save an image to storage.
        /// </summary>
        /// <param name="imageStream">The stream of the image to save.</param>
        /// <param name="fileExtension">The file extension of the image.</param>
        /// <returns>The URI of the saved image.</returns>
        /// <exception cref="System.Exception">Thrown if saving the image fails.<exception>
        Task<string> SaveImage(Stream imageStream, string fileExtension);

        /// <summary>
        /// Delete an image from storage.
        /// </summary>
        /// <param name="fileUri">The URI of the image to delete.</param>
        /// <returns>True/false for success.</returns>
        Task<bool> DeleteImage(string fileUri);
    }
}
