using MediatR;
using UserManager.Application.Commands.Requests;
using UserManager.Domain.Entities;
using UserManager.Domain.Interfaces;

namespace UserManager.Application.Commands.Handlers
{
    public class CreateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<CreateUserCommand, int>
    {
        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Name = request.Name,
                Username = request.Username,
                Email = request.Email,
                Phone = request.Phone,
                Website = request.Website
            };

            await userRepository.AddAsync(user);

            return user.Id;
        }
    }
}
