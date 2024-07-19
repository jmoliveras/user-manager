using MediatR;

namespace UserManager.Application.Commands.Requests
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}