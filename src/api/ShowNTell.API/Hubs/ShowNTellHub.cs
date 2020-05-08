using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ShowNTell.API.Hubs
{
    public class ShowNTellHub : Hub
    {
        public const string IMAGE_POST_CREATED = "IMAGE_POST_CREATED";
        public const string IMAGE_POST_UPDATED = "IMAGE_POST_UPDATED";
        public const string IMAGE_POST_DELETED = "IMAGE_POST_DELETED";
    }
}