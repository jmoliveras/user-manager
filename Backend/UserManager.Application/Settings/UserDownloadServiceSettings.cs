namespace UserManager.Application.Settings
{
    public class UserDownloadServiceSettings
    {
        public required string UserApiUrl { get; set; }
        public int ExecutionIntervalMinutes { get; set; }
    }
}
