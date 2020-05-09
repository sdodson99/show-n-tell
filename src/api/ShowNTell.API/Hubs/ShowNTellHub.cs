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

        public const string PROFILE_FOLLOW = "PROFILE_FOLLOW";
        public const string PROFILE_UNFOLLOW = "PROFILE_UNFOLLOW";

        public const string IMAGE_POST_LIKE = "IMAGE_POST_LIKE";
        public const string IMAGE_POST_UNLIKE = "IMAGE_POST_UNLIKE";

        public const string IMAGE_POST_COMMENT_CREATED = "IMAGE_POST_COMMENT_CREATED";
        public const string IMAGE_POST_COMMENT_UPDATED = "IMAGE_POST_COMMENT_UPDATED";
        public const string IMAGE_POST_COMMENT_DELETED = "IMAGE_POST_COMMENT_DELETED";

    }
}