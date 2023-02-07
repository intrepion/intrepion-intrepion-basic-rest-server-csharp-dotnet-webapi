using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IntrepionApp.WebApi.Authentication.User;

namespace IntrepionApp.WebApi.Authentication.LogOut;

public interface ILogOutsController
{
    public Task<IActionResult> MakeLogOutAsync();
}

[ApiController]
[Route("{controller}")]
public class LogOutsController : ControllerBase, ILogOutsController
{
    private readonly ILogOutsRepository _LogOutsRepository;
    private readonly UserManager<UserEntity> _userManager;

    public LogOutsController(ILogOutsRepository LogOutsRepository, UserManager<UserEntity> userManager)
    {
        _LogOutsRepository = LogOutsRepository;
        _userManager = userManager;
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> MakeLogOutAsync()
    {
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);

        var makeLogOutResponse = await _LogOutsRepository.MakeLogOutAsync(currentUser);

        if (makeLogOutResponse is null)
        {
            return BadRequest();
        }

        return Ok(makeLogOutResponse);
    }
}
