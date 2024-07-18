using Application.Commands.Requests;
using MediatR;
using UserManager.Domain.Interfaces;


namespace Application.Commands.Handlers
{
    public class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand>
    {
        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await userRepository.DeleteAsync(request.Id);
        }
    }
}