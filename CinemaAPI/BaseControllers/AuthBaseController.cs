using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAPI.BaseControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public abstract class AuthBaseController : ControllerBase
    {
    }
}
