using MediatR;
using UserManager.Application.DTO;

namespace UserManager.Application.Commands.Requests
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public required UserDto UserDto { get; set; }
    }
}
