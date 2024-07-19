namespace UserManager.Application.Interfaces
{
    public interface IUserDownloadService
    {
        Task DownloadAndSaveUsersAsync();     
    }
}
