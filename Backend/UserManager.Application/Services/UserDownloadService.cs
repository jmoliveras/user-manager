using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using UserManager.Application.DTO;
using UserManager.Application.Interfaces;
using UserManager.Application.Settings;
using UserManager.Domain.Entities;
using UserManager.Domain.Interfaces;

namespace UserManager.Application.Services
{

    public class UserDownloadService(IServiceProvider serviceProvider, IOptions<UserDownloadServiceSettings> settings,
        IHttpClientFactory httpClientFactory, IMapper mapper) : BackgroundService, IUserDownloadService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DownloadAndSaveUsersAsync();
                await Task.Delay(TimeSpan.FromMinutes(settings.Value.ExecutionIntervalMinutes), stoppingToken);
            }
        }

        public async Task DownloadAndSaveUsersAsync()
        {
            var httpClient = httpClientFactory.CreateClient("UserApiClient");
            var users = await httpClient.GetFromJsonAsync<UserDto[]>("users");

            if (users != null)
            {
                await SaveUsers(users.Select(mapper.Map<User>));
            }
        }

        private async Task SaveUsers(IEnumerable<User> users)
        {
            using var scope = serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            await userRepository.DeleteAllAsync();
            await userRepository.AddRangeAsync(users);
        }
    }
}
