using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IntrepionApp.WebApi.Authentication.User;

public interface IUsersController
{
    public Task<IActionResult> AllUsersAsync();
    public Task<IActionResult> EditUserAsync(string userName, [FromBody] EditUserRequest editUserRequest);
    public Task<IActionResult> MakeUserAsync([FromBody] MakeUserRequest makeUserRequest);
    public Task<IActionResult> RemoveUserAsync(string userName);
    public Task<IActionResult> ViewUserAsync(string userName);
}

[ApiController]
[Route("{controller}")]
public class UsersController : ControllerBase, IUsersController
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly IUsersRepository _usersRepository;

    public UsersController(UserManager<UserEntity> userManager, IUsersRepository usersRepository)
    {
        _userManager = userManager;
        _usersRepository = usersRepository;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> AllUsersAsync()
    {
        var allUsersResponse = await _usersRepository.AllUsersAsync();

        if (allUsersResponse is null)
        {
            return BadRequest();
        }

        return Ok(allUsersResponse);
    }

    [AllowAnonymous]
    [HttpPut]
    [Route("{userName}")]
    public async Task<IActionResult> EditUserAsync(string userName, [FromBody] EditUserRequest editUserRequest)
    {
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);

        var editUserResponse = await _usersRepository.EditUserAsync(currentUser, userName, editUserRequest);

        if (editUserResponse is null)
        {
            return BadRequest();
        }

        return Ok(editUserResponse);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> MakeUserAsync([FromBody] MakeUserRequest makeUserRequest)
    {
        var makeUserResponse = await _usersRepository.MakeUserAsync(makeUserRequest);

        if (makeUserResponse is null)
        {
            return BadRequest();
        }

        return Ok(makeUserResponse);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    [Route("{userName}")]
    public async Task<IActionResult> RemoveUserAsync(string userName)
    {
        var removeUserResponse = await _usersRepository.RemoveUserAsync(userName);

        if (removeUserResponse is null)
        {
            return BadRequest();
        }

        return Ok(removeUserResponse);
    }

    [HttpGet]
    [Route("{userName}")]
    public async Task<IActionResult> ViewUserAsync(string userName)
    {
        var viewUserResponse = await _usersRepository.ViewUserAsync(userName);

        if (viewUserResponse is null)
        {
            return BadRequest();
        }

        return Ok(viewUserResponse);
    }
}
