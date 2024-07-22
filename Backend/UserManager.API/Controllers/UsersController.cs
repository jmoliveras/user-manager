using UserManager.Application.Commands.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserManager.Application.DTO;
using UserManager.Application.Queries.Requests;
using UserManager.API.Controllers.Base;

namespace UserManager.API.Controllers
{      
    public class UsersController(IMediator mediator) : ApiBaseController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await mediator.Send(new GetAllUsersQuery());
            return Ok(users);
        }        

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest();
            }

            var result = await mediator.Send(new UpdateUserCommand { UserDto = userDto });
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }      

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await mediator.Send(new DeleteUserCommand { Id = id });
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
