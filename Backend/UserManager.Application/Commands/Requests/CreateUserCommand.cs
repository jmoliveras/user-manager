using MediatR;

namespace UserManager.Application.Commands.Requests
{
    public class CreateUserCommand: IRequest<int>
    {
        public required string Name { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Website { get; set; }
    }
}
