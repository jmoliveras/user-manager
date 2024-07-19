using UserManager.Application.Commands.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserManager.Application.DTO;
using UserManager.Application.Queries.Requests;

namespace UserManager.API.Controllers
{  
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _mediator.Send(new GetAllUsersQuery());
            return Ok(users);
        }        

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(new UpdateUserCommand { UserDto = userDto });
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }      

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _mediator.Send(new DeleteUserCommand { Id = id });
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
