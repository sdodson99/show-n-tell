using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ShowNTell.AzureStorage.Services.BlobClientFactories;
using ShowNTell.AzureStorage.Services.BlobClients;
using ShowNTell.Domain.Services;
using ShowNTell.Domain.Services.ImageStorages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.AzureStorage.Services
{
    public class AzureBlobImageStorage : IImageStorage
    {
        private readonly IBlobClientFactory _blobClientFactory;

        public AzureBlobImageStorage(IBlobClientFactory blobClientFactory)
        {
            _blobClientFactory = blobClientFactory;
        }

        public async Task<string> SaveImage(Stream imageStream, string fileExtension)
        {
            IBlobClient client = await _blobClientFactory.CreateBlobClient();

            string imageName = Guid.NewGuid().ToString() + fileExtension;
            await client.UploadBlobAsync(imageName, imageStream);

            return Path.Combine(client.Uri.AbsoluteUri, imageName);
        }

        public async Task<bool> DeleteImage(string fileUri)
        {
            IBlobClient client = await _blobClientFactory.CreateBlobClient();

            string imageName = fileUri.Substring(client.Uri.AbsoluteUri.Length).Trim('/', '\\');
            Response deleteResponse = await client.DeleteBlobAsync(imageName);

            return deleteResponse.Status.ToString().StartsWith('2');
        }
    }
}
