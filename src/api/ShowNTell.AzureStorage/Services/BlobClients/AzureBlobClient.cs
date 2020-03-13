using System;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ShowNTell.AzureStorage.Services.BlobClients
{
    public class AzureBlobClient : IBlobClient
    {
        private readonly BlobContainerClient _client;

        public AzureBlobClient(BlobContainerClient client)
        {
            _client = client;
        }

        public Uri Uri => _client.Uri;

        public async Task<Response<BlobContentInfo>> UploadBlobAsync(string blobName, Stream stream)
        {
            return await _client.UploadBlobAsync(blobName, stream);
        }

        public async Task<Response> DeleteBlobAsync(string blobName)
        {
            return await _client.DeleteBlobAsync(blobName);
        }
    }
}