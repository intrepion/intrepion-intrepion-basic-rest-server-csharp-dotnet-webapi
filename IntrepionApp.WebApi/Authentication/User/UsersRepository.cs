using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IntrepionApp.WebApi.Authentication.Role;
using IntrepionApp.WebApi.Database;

namespace IntrepionApp.WebApi.Authentication.User;

public interface IUsersRepository
{
    public Task<AllUsersResponse?> AllUsersAsync();
    public Task<EditUserResponse?> EditUserAsync(UserEntity? currentUser, string userName, EditUserRequest editUserRequest);
    public Task<MakeUserResponse?> MakeUserAsync(MakeUserRequest makeUserRequest);
    public Task<RemoveUserResponse?> RemoveUserAsync(string userName);
    public Task<ViewUserResponse?> ViewUserAsync(string userName);
}

public class UsersRepository : IUsersRepository
{
    private readonly ApplicationDatabaseContext _applicationDatabaseContext;
    private readonly RoleManager<RoleEntity> _roleManager;
    private readonly UserManager<UserEntity> _userManager;

    public UsersRepository(
        ApplicationDatabaseContext applicationDatabaseContext,
        RoleManager<RoleEntity> roleManager,
        UserManager<UserEntity> userManager)
    {
        _applicationDatabaseContext = applicationDatabaseContext;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<AllUsersResponse?> AllUsersAsync()
    {
        var data = await _applicationDatabaseContext.Users.Select(user => new AllUsersResponseUser()
        {
            UserName = user.UserName,
        }).ToListAsync();

        if (data is null)
        {
            return null;
        }

        return new AllUsersResponse()
        {
            Users = data,
        };
    }

    public async Task<EditUserResponse?> EditUserAsync(UserEntity? currentUser, string userName, EditUserRequest editUserRequest)
    {
        Console.WriteLine("EditUserAsync");
        if (userName is null)
        {
            Console.WriteLine("userName is null");
            return null;
        }

        if (editUserRequest is null)
        {
            Console.WriteLine("editUserRequest is null");
            return null;
        }

        userName = userName.Trim();

        var userEntity = await _userManager.FindByNameAsync(userName);

        if (userEntity is null)
        {
            Console.WriteLine("userEntity is null");
            return null;
        }

        if (currentUser is null)
        {
            Console.WriteLine("currentUser is null");
            return null;
        }

        if (currentUser.UserName is null)
        {
            Console.WriteLine("currentUser.UserName is null");
            return null;
        }

        if (!currentUser.UserName.Equals(userName))
        {
            Console.WriteLine("currentUser.UserName is not equal to userName");
            if (!_userManager.IsInRoleAsync(currentUser, "Admin").Result)
            {
                Console.WriteLine("currentUser is not admin");
                return null;
            }
        }

        if (!(String.IsNullOrWhiteSpace(editUserRequest.PasswordCurrent)
            || String.IsNullOrWhiteSpace(editUserRequest.Confirm)
            || String.IsNullOrWhiteSpace(editUserRequest.Password)))
        {
            if (!editUserRequest.Confirm.Equals(editUserRequest.Password))
            {
                Console.WriteLine("editUserRequest.Confirm is not equal to editUserRequest.Password");
                return null;
            }

            await _userManager.ChangePasswordAsync(userEntity, editUserRequest.PasswordCurrent, editUserRequest.Password);
        }

        if (!String.IsNullOrWhiteSpace(editUserRequest.Email))
        {
            editUserRequest.Email = editUserRequest.Email.Trim();
            userEntity.Email = editUserRequest.Email;
        }

        if (!String.IsNullOrWhiteSpace(editUserRequest.UserName))
        {
            editUserRequest.UserName = editUserRequest.UserName.Trim();
            userEntity.UserName = editUserRequest.UserName;
        }

        var result = await _userManager.UpdateAsync(userEntity);

        if (!result.Succeeded)
        {
            Console.WriteLine("result is not succeeded");
            return null;
        }

        return new EditUserResponse
        {
            Email = userEntity.Email,
            UserName = userEntity.UserName,
        };
    }

    public async Task<MakeUserResponse?> MakeUserAsync(MakeUserRequest makeUserRequest)
    {
        if (makeUserRequest is null)
        {
            return null;
        }

        if (String.IsNullOrWhiteSpace(makeUserRequest.Confirm))
        {
            return null;
        }

        makeUserRequest.Confirm = makeUserRequest.Confirm.Trim();

        if (String.IsNullOrWhiteSpace(makeUserRequest.Email))
        {
            return null;
        }

        makeUserRequest.Email = makeUserRequest.Email.Trim();

        if (String.IsNullOrWhiteSpace(makeUserRequest.Password))
        {
            return null;
        }

        makeUserRequest.Password = makeUserRequest.Password.Trim();

        if (String.IsNullOrWhiteSpace(makeUserRequest.UserName))
        {
            return null;
        }

        makeUserRequest.UserName = makeUserRequest.UserName.Trim();

        if (!makeUserRequest.Confirm.Equals(makeUserRequest.Password))
        {
            return null;
        }

        var user = new UserEntity
        {
            Email = makeUserRequest.Email,
            UserName = makeUserRequest.UserName,
        };

        var result = await _userManager.CreateAsync(user, makeUserRequest.Password);

        if (!result.Succeeded)
        {
            return null;
        }

        var regularRole = await _roleManager.FindByNameAsync("Regular");

        if (regularRole is null)
        {
            return null;
        }

        if (regularRole.Name is null)
        {
            return null;
        }

        await _userManager.AddToRolesAsync(user, new List<string> { regularRole.Name });

        return new MakeUserResponse
        {
            UserName = user.UserName,
        };
    }

    public async Task<RemoveUserResponse?> RemoveUserAsync(string userName)
    {
        if (String.IsNullOrWhiteSpace(userName))
        {
            return null;
        }

        var userEntity = await _userManager.FindByNameAsync(userName);

        if (userEntity is null)
        {
            return null;
        }

        var result = await _userManager.DeleteAsync(userEntity);

        if (!result.Succeeded)
        {
            return null;
        }

        return new RemoveUserResponse
        {
        };
    }

    public async Task<ViewUserResponse?> ViewUserAsync(string userName)
    {
        if (String.IsNullOrWhiteSpace(userName))
        {
            return null;
        }

        var userEntity = await _applicationDatabaseContext.Users.SingleOrDefaultAsync(user => user.UserName == userName);

        if (userEntity is null)
        {
            return null;
        }

        return new ViewUserResponse
        {
            Email = userEntity.Email,
            UserName = userEntity.UserName,
        };
    }
}
