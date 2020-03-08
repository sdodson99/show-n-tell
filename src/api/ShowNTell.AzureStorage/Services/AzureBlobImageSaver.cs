using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ShowNTell.AzureStorage.Services.BlobClientFactories;
using ShowNTell.AzureStorage.Services.BlobClients;
using ShowNTell.Domain.Services;
using ShowNTell.Domain.Services.ImageSavers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.AzureStorage.Services
{
    public class AzureBlobImageSaver : IImageSaver
    {
        private readonly IBlobClientFactory _blobClientFactory;

        public AzureBlobImageSaver(IBlobClientFactory blobClientFactory)
        {
            _blobClientFactory = blobClientFactory;
        }

        public async Task<string> SaveImage(Stream imageStream)
        {
            IBlobClient client = await _blobClientFactory.CreateBlobClient();

            string imageId = Guid.NewGuid().ToString();
            await client.UploadBlobAsync(imageId, imageStream);

            return Path.Combine(client.Uri.AbsoluteUri, imageId);
        }
    }
}
