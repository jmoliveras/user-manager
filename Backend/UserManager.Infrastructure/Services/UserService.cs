using System.Net.Http.Json;
using UserManager.Domain.Entities;
using UserManager.Domain.Interfaces;

namespace UserManager.Infrastructure.Services
{

    public class UserService(HttpClient httpClient, IUserRepository userRepository)
    {
        public async Task DownloadUsersAsync()
        {
            var users = await httpClient.GetFromJsonAsync<List<User>>("https://jsonplaceholder.typicode.com/users");
            var dbUsers = await userRepository.GetAllAsync();

            if (users != null)
            {
                foreach (var user in users)
                {
                    var existingUser = dbUsers.SingleOrDefault(u => u.Id == user.Id);

                    if (existingUser == null)
                    {
                        await userRepository.AddAsync(user);
                    }
                    else
                    {
                        existingUser.Name = user.Name;
                        existingUser.Username = user.Username;
                        existingUser.Email = user.Email;
                        existingUser.Phone = user.Phone;
                        existingUser.Website = user.Website;

                        await userRepository.UpdateAsync(existingUser);
                    }
                }
            }
        }
    }
}
