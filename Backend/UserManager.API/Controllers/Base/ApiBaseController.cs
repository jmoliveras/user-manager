using Microsoft.AspNetCore.Mvc;

namespace UserManager.API.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public abstract class ApiBaseController : ControllerBase
    {
    }
}
