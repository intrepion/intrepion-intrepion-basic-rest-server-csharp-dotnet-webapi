using System.Text.Json.Serialization;

namespace IntrepionApp.WebApi.Authentication.User;

public class ViewUserResponse
{
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("userName")]
    public string? UserName { get; set; }
}
