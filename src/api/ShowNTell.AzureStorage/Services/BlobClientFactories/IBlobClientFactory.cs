using System.Threading.Tasks;
using ShowNTell.AzureStorage.Services.BlobClients;

namespace ShowNTell.AzureStorage.Services.BlobClientFactories
{
    public interface IBlobClientFactory
    {
         Task<IBlobClient> CreateBlobClient();
    }
}