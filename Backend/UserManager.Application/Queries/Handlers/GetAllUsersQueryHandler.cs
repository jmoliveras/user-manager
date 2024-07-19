using MediatR;
using UserManager.Application.DTO;
using UserManager.Domain.Interfaces;
using AutoMapper;
using UserManager.Application.Queries.Requests;

namespace UserManager.Application.Queries.Handlers
{
    public class GetAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await userRepository.GetAllAsync();
            return mapper.Map<List<UserDto>>(users);
        }
    }
}
