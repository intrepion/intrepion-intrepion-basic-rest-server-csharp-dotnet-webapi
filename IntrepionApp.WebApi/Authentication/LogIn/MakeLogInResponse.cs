using System.Text.Json.Serialization;

namespace IntrepionApp.WebApi.Authentication.LogIn;

public class MakeLogInResponse
{
    [JsonPropertyName("userName")]
    public string? UserName { get; set; }
}
