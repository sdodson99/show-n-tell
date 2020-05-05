namespace ShowNTell.API.Models
{
    public enum ShowNTellAccessMode
    {
        None,
        Read,
        Write,
        All
    }

    public class ShowNTellConfiguration
    {
        public string DatabaseConnectionString { get; set; }
        public string ApplicationInsightsKey { get; set; }
        public string BlobStorageConnectionString { get; set; }
        public string BaseUrl { get; set; }
        public ShowNTellAccessMode ShowNTellAccessMode { get; set; } = ShowNTellAccessMode.All;
        public bool ReadAccessModeEnabled => ShowNTellAccessMode == ShowNTellAccessMode.Read || ShowNTellAccessMode == ShowNTellAccessMode.All;
        public bool WriteAccessModeEnabled => ShowNTellAccessMode == ShowNTellAccessMode.Write || ShowNTellAccessMode == ShowNTellAccessMode.All;
    }
}