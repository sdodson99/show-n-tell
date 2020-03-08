using System;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs.Models;

namespace ShowNTell.AzureStorage.Services.BlobClients
{
    public interface IBlobClient
    {
        Uri Uri { get; }
        
        Task<Response<BlobContentInfo>> UploadBlobAsync(string blobName, FileStream stream);
    }
}