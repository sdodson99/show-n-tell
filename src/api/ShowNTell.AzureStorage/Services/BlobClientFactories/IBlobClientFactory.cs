using System.Threading.Tasks;
using ShowNTell.AzureStorage.Services.BlobClients;

namespace ShowNTell.AzureStorage.Services.BlobClientFactories
{
    /// <summary>
    /// A factory for creating blob clients.
    /// </summary>
    public interface IBlobClientFactory
    {
        /// <summary>
        /// Create a new blob client.
        /// </summary>
        /// <returns>The created blob client.</returns>
        Task<IBlobClient> CreateBlobClient();
    }
}