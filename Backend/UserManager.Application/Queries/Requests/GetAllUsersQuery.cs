using MediatR;
using UserManager.Application.DTO;

namespace UserManager.Application.Queries.Requests
{
    public class GetAllUsersQuery : IRequest<List<UserDto>>
    {
    }
}
