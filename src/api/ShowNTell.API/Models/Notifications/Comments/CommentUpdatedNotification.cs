using ShowNTell.API.Hubs;
using ShowNTell.API.Models.Responses;

namespace ShowNTell.API.Models.Notifications.Comments
{
    public class CommentUpdatedNotification : ISignalRNotification
    {
        public string MethodName => ShowNTellHub.IMAGE_POST_COMMENT_UPDATED;

        public object Data => CommentResponse;

        public CommentResponse CommentResponse { get; }

        public CommentUpdatedNotification(CommentResponse commentResponse)
        {
            CommentResponse = commentResponse;
        }
    }
}