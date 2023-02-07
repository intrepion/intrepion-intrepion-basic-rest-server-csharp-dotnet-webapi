using System.Text.Json.Serialization;

namespace IntrepionApp.WebApi.Authentication.LogIn;

public class MakeLogInRequest
{
    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("rememberMe")]
    public bool RememberMe { get; set; }

    [JsonPropertyName("userName")]
    public string? UserName { get; set; }
}
