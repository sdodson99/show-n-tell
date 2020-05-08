namespace ShowNTell.API.Models.Notifications
{
    public interface ISignalRNotification : INotification
    {
        string MethodName{ get; }
        object Data { get; }
    }
}