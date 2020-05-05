namespace ShowNTell.API.Models.Responses
{
    public enum ErrorCode
    {
        READ_MODE_DISABLED,
        WRITE_MODE_DISABLED
    }

    public class ErrorResponse
    {
        public ErrorCode Code { get; set; }
        public string Message { get; set; }
    }
}