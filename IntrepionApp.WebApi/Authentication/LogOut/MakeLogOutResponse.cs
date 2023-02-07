using System.Text.Json.Serialization;

namespace IntrepionApp.WebApi.Authentication.LogOut;

public class MakeLogOutResponse
{
    [JsonPropertyName("userName")]
    public string? UserName { get; set; }
}
