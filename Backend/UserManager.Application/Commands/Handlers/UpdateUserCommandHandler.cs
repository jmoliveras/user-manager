using AutoMapper;
using MediatR;
using UserManager.Application.Commands.Requests;
using UserManager.Domain.Entities;
using UserManager.Domain.Interfaces;

namespace UserManager.Application.Commands.Handlers
{
    public class UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<UpdateUserCommand, bool>
    {
        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = mapper.Map<User>(request.UserDto);
            var existingUser = await userRepository.GetByIdAsync(user.Id);
            
            if (existingUser == null)
            {
                return false;
            }

            mapper.Map(request.UserDto, existingUser);
            await userRepository.UpdateAsync(existingUser);

            return true;
        }
    }
}