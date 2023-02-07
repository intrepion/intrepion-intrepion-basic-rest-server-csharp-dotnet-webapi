using System.Text.Json.Serialization;

namespace IntrepionApp.WebApi.Authentication.User;

public class MakeUserRequest
{
    [JsonPropertyName("confirm")]
    public string? Confirm { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("userName")]
    public string? UserName { get; set; }
}
