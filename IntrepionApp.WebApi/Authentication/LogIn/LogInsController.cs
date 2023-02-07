using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IntrepionApp.WebApi.Authentication.User;

namespace IntrepionApp.WebApi.Authentication.LogIn;

public interface ILogInsController
{
    public Task<IActionResult> MakeLogInAsync([FromBody] MakeLogInRequest makeLogInRequest);
}

[ApiController]
[Route("{controller}")]
public class LogInsController : ControllerBase, ILogInsController
{
    private readonly ILogInsRepository _loginsRepository;
    private readonly UserManager<UserEntity> _userManager;

    public LogInsController(ILogInsRepository loginsRepository, UserManager<UserEntity> userManager)
    {
        _loginsRepository = loginsRepository;
        _userManager = userManager;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> MakeLogInAsync([FromBody] MakeLogInRequest makeLogInRequest)
    {
        var makeLogInResponse = await _loginsRepository.MakeLogInAsync(makeLogInRequest);

        if (makeLogInResponse is null)
        {
            return BadRequest();
        }

        return Ok(makeLogInResponse);
    }
}
