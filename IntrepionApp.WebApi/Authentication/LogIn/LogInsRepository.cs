using Microsoft.AspNetCore.Identity;
using IntrepionApp.WebApi.Authentication.User;

namespace IntrepionApp.WebApi.Authentication.LogIn;

public interface ILogInsRepository
{
    public Task<MakeLogInResponse?> MakeLogInAsync(MakeLogInRequest makeLogInRequest);
}

public class LogInsRepository : ILogInsRepository
{
    private readonly SignInManager<UserEntity> _signInManager;

    public LogInsRepository(SignInManager<UserEntity> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<MakeLogInResponse?> MakeLogInAsync(MakeLogInRequest makeLogInRequest)
    {
        if (makeLogInRequest is null)
        {
            Console.WriteLine("makeLogInRequest is null");
            return null;
        }

        if (String.IsNullOrWhiteSpace(makeLogInRequest.Password))
        {
            Console.WriteLine("makeLogInRequest.Password is null");
            return null;
        }

        if (String.IsNullOrWhiteSpace(makeLogInRequest.UserName))
        {
            Console.WriteLine("makeLogInRequest.UserName is null");
            return null;
        }

        var result = await _signInManager.PasswordSignInAsync(makeLogInRequest.UserName, makeLogInRequest.Password, makeLogInRequest.RememberMe, false);

        if (!result.Succeeded)
        {
            Console.WriteLine("result.Succeeded is false");
            return null;
        }

        return new MakeLogInResponse
        {
            UserName = makeLogInRequest.UserName,
        };
    }
}
