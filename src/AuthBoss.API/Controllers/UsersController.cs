using AuthBoss.Communication.Requests.User;
using Microsoft.AspNetCore.Mvc;

namespace AuthBoss.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RequestRegisterUserJson request)
    {
        return Ok();
    }
}
