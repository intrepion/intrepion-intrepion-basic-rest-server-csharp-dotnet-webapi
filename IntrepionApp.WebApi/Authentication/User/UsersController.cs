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

    [Authorize]
    [HttpPut]
    [Route("{userName}")]
    public async Task<IActionResult> EditUserAsync(string userName, [FromBody] EditUserRequest editUserRequest)
    {
        if (editUserRequest is null)
        {
            Console.WriteLine("editUserRequest is null");
            return BadRequest();
        }

        if (String.IsNullOrWhiteSpace(userName))
        {
            Console.WriteLine("userName is null");
            return BadRequest();
        }

        if (HttpContext.User is null)
        {
            Console.WriteLine("HttpContext.User is null");
            return BadRequest();
        }

        Console.WriteLine("EditUserAsync");
        Console.WriteLine(HttpContext.User.Identity);
        Console.WriteLine(HttpContext.User.Identity?.Name);
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);
        Console.WriteLine(currentUser);
        Console.WriteLine(currentUser?.UserName);
        UserEntity? currentUser2 = null;
        
        if (currentUser == null)
        {
            Console.WriteLine("currentUser is null");
            return BadRequest();
        }

        if (HttpContext.User.Identity?.Name is not null)
        {
            currentUser2 = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }

        if (currentUser2 is not null)
        {
            Console.WriteLine(currentUser2);
            Console.WriteLine(currentUser2?.UserName);
        }

        var editUserResponse = await _usersRepository.EditUserAsync(currentUser, userName, editUserRequest);

        if (editUserResponse is null)
        {
            Console.WriteLine("editUserResponse is null");
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
