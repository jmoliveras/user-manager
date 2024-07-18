using UserManager.Domain.Entities;
using UserManager.Domain.Interfaces;
using UserManager.Infrastructure.Data;

namespace UserManager.Infrastructure.Repositories
{
    public class UserRepository(ApplicationDbContext context) : GenericRepository<User>(context), IUserRepository
    {
    }
}
