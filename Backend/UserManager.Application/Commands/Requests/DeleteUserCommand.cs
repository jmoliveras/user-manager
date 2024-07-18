using MediatR;

namespace Application.Commands.Requests
{
    public class DeleteUserCommand : IRequest
    {
        public int Id { get; set; }
    }
}