using UserManager.Application.Commands.Requests;
using MediatR;
using UserManager.Domain.Interfaces;

namespace UserManager.Application.Commands.Handlers
{
    public class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand, bool>
    {
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await userRepository.GetByIdAsync(request.Id);

            if (existingUser == null)
            {
                return false;
            }

            await userRepository.DeleteAsync(request.Id);

            return true;
        }
    }
}