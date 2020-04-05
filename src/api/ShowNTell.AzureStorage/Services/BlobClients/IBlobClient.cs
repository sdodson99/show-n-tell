using System;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs.Models;

namespace ShowNTell.AzureStorage.Services.BlobClients
{
    /// <summary>
    /// A client for uploading and deleting blobs.
    /// </summary>
    public interface IBlobClient
    {
        Uri Uri { get; }
        
        /// <summary>
        /// Upload a new blob.
        /// </summary>
        /// <param name="blobName">The name of the new blob.</param>
        /// <param name="stream">The content of the blob.</param>
        /// <returns>The blob upload result.</returns>
        Task<Response<BlobContentInfo>> UploadBlobAsync(string blobName, Stream stream);

        /// <summary>
        /// Delete a blob.
        /// </summary>
        /// <param name="blobName">The name of the blob to delete.</param>
        /// <returns>The blob delete result.</returns>
        Task<Response> DeleteBlobAsync(string blobName);
    }
}