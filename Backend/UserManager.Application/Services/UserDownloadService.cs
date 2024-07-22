using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using UserManager.Application.DTO;
using UserManager.Application.Interfaces;
using UserManager.Application.Settings;
using UserManager.Domain.Entities;
using UserManager.Domain.Interfaces;

namespace UserManager.Application.Services
{

    public class UserDownloadService(IServiceProvider serviceProvider,
        IOptions<UserDownloadServiceSettings> settings,
        IHttpClientFactory httpClientFactory,
        IMapper mapper) : BackgroundService, IUserDownloadService
    {
        private readonly UserDownloadServiceSettings _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(_settings.ExecutionIntervalMinutes));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await DownloadAndSaveUsersAsync();
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
