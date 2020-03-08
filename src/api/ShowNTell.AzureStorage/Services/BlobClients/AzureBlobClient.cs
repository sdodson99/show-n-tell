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
        private BlobContainerClient _client;

        public AzureBlobClient(BlobContainerClient client)
        {
            _client = client;
        }

        public Uri Uri => _client.Uri;

        public async Task<Response<BlobContentInfo>> UploadBlobAsync(string blobName, FileStream stream)
        {
            return await _client.UploadBlobAsync(blobName, stream);
        }
    }
}