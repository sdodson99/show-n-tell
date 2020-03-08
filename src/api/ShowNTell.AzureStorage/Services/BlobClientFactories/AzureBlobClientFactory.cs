using System.Threading.Tasks;
using Azure.Storage.Blobs;
using ShowNTell.AzureStorage.Services.BlobClients;

namespace ShowNTell.AzureStorage.Services.BlobClientFactories
{
    public class AzureBlobClientFactory : IBlobClientFactory
    {
        private readonly string _connectionString;
        private readonly string _blobContainerName;

        public AzureBlobClientFactory(string connectionString, string blobContainerName)
        {
            _connectionString = connectionString;
            _blobContainerName = blobContainerName;
        }

        public async Task<IBlobClient> CreateBlobClient()
        {
            BlobServiceClient blobService = new BlobServiceClient(_connectionString);
            BlobContainerClient client = blobService.GetBlobContainerClient(_blobContainerName);
            await client.CreateIfNotExistsAsync();
            
            return new AzureBlobClient(client);
        }
    }
}