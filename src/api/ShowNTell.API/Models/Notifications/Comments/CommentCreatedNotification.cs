using ShowNTell.API.Hubs;
using ShowNTell.API.Models.Responses;

namespace ShowNTell.API.Models.Notifications.Comments
{
    public class CommentCreatedNotification : ISignalRNotification
    {
        public string MethodName => ShowNTellHub.IMAGE_POST_COMMENT_CREATED;

        public object Data => CommentResponse;

        public CommentResponse CommentResponse { get; }

        public CommentCreatedNotification(CommentResponse commentResponse)
        {
            CommentResponse = commentResponse;
        }
    }
}