using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IntrepionApp.WebApi.Authentication.Role;
using IntrepionApp.WebApi.Authentication.User;
using IntrepionApp.WebApi.Authentication.UserRole;

namespace IntrepionApp.WebApi.Database;

public class ApplicationDatabaseContext : IdentityDbContext<UserEntity, RoleEntity, Guid>
{
    public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var adminRole = new RoleEntity
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            NormalizedName = "ADMIN"
        };

        var regularRole = new RoleEntity
        {
            Id = Guid.NewGuid(),
            Name = "Regular",
            NormalizedName = "REGULAR"
        };

        builder.Entity<RoleEntity>().HasData(new List<RoleEntity>
        {
            adminRole,
            regularRole,
        });

        var hasher = new PasswordHasher<UserEntity>();

        var adminUser = new UserEntity
        {
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            Email = "intrepion@gmail.com",
            Id = Guid.NewGuid(),
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = "admin",
        };

        adminUser.NormalizedEmail = adminUser.Email.ToUpper();
        adminUser.NormalizedUserName = adminUser.UserName.ToUpper();
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "adminP@ssw0rd");

        var intrepionUser = new UserEntity
        {
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            Email = "intrepion@gmail.com",
            Id = Guid.NewGuid(),
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = "intrepion",
        };

        intrepionUser.NormalizedEmail = intrepionUser.Email.ToUpper();
        intrepionUser.NormalizedUserName = intrepionUser.UserName.ToUpper();
        intrepionUser.PasswordHash = hasher.HashPassword(intrepionUser, "intrepionP@ssw0rd");

        var regularUser = new UserEntity
        {
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            Email = "intrepion@gmail.com",
            Id = Guid.NewGuid(),
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = "regular",
        };

        regularUser.NormalizedEmail = regularUser.Email.ToUpper();
        regularUser.NormalizedUserName = regularUser.UserName.ToUpper();
        regularUser.PasswordHash = hasher.HashPassword(regularUser, "regularP@ssw0rd");

        var userUser = new UserEntity
        {
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            Email = "intrepion@gmail.com",
            Id = Guid.NewGuid(),
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = "user",
        };

        userUser.NormalizedEmail = userUser.Email.ToUpper();
        userUser.NormalizedUserName = userUser.UserName.ToUpper();
        userUser.PasswordHash = hasher.HashPassword(userUser, "userP@ssw0rd");

        builder.Entity<UserEntity>().HasData(
            adminUser,
            intrepionUser,
            regularUser,
            userUser
        );

        builder.Entity<UserRoleEntity>().HasData(
            new UserRoleEntity
            {
                RoleId = adminRole.Id,
                UserId = adminUser.Id,
            },
            new UserRoleEntity
            {
                RoleId = regularRole.Id,
                UserId = regularUser.Id,
            },
            new UserRoleEntity
            {
                RoleId = regularRole.Id,
                UserId = userUser.Id,
            }
        );
    }
}
