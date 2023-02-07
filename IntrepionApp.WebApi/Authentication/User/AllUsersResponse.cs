using System.Text.Json.Serialization;

namespace IntrepionApp.WebApi.Authentication.User;

public class AllUsersResponseUser
{
    [JsonPropertyName("userName")]
    public string? UserName { get; set; }
}

public class AllUsersResponse
{
    [JsonPropertyName("users")]
    public List<AllUsersResponseUser>? Users { get; set; }
}
