using Microsoft.AspNetCore.Identity;
using IntrepionApp.WebApi.Authentication.User;

namespace IntrepionApp.WebApi.Authentication.LogOut;

public interface ILogOutsRepository
{
    public Task<MakeLogOutResponse?> MakeLogOutAsync(UserEntity? currentUser);
}

public class LogOutsRepository : ILogOutsRepository
{
    private readonly SignInManager<UserEntity> _signInManager;

    public LogOutsRepository(SignInManager<UserEntity> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<MakeLogOutResponse?> MakeLogOutAsync(UserEntity? currentUser)
    {
        if (currentUser is null)
        {
            return new MakeLogOutResponse();
        }

        var userName = currentUser.UserName;

        await _signInManager.SignOutAsync();

        return new MakeLogOutResponse
        {
            UserName = userName,
        };
    }
}
