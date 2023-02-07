using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using IntrepionApp.WebApi.Authentication.LogIn;
using IntrepionApp.WebApi.Authentication.LogOut;

namespace IntrepionApp.Tests.Endpoints;

public class TestLogInsEndpoints : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TestLogInsEndpoints(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task LogIns_Endpoints()
    {
        // Arrange
        var client = _factory.CreateClient();
        var emptyContent = new StringContent("", Encoding.UTF8, "application/json");

        var adminMakeLogInRequest = new MakeLogInRequest
        {
            Password = "adminP@ssw0rd",
            RememberMe = true,
            UserName = "admin",
        };
        var adminMakeLogInRequestString = JsonSerializer.Serialize(adminMakeLogInRequest);
        var adminMakeLogInRequestContent = new StringContent(adminMakeLogInRequestString, Encoding.UTF8, "application/json");
        var userMakeLogInRequest = new MakeLogInRequest
        {
            Password = "userP@ssw0rd",
            RememberMe = true,
            UserName = "user",
        };
        var userMakeLogInRequestString = JsonSerializer.Serialize(userMakeLogInRequest);
        var userMakeLogInRequestContent = new StringContent(userMakeLogInRequestString, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/LogOuts", emptyContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        var makeLogOutResponse = JsonSerializer.Deserialize<MakeLogOutResponse>(responseContent);
        Assert.NotNull(makeLogOutResponse);
        Assert.Null(makeLogOutResponse.UserName);

        // Act
        response = await client.PostAsync("/LogIns", userMakeLogInRequestContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        var makeLogInResponse = JsonSerializer.Deserialize<MakeLogInResponse>(responseContent);
        Assert.NotNull(makeLogInResponse);
        Assert.NotNull(makeLogInResponse.UserName);
        makeLogInResponse.UserName.Should().Be(userMakeLogInRequest.UserName);

        // Act
        response = await client.PostAsync("/LogOuts", emptyContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        makeLogOutResponse = JsonSerializer.Deserialize<MakeLogOutResponse>(responseContent);
        Assert.NotNull(makeLogOutResponse);
        Assert.NotNull(makeLogOutResponse.UserName);
        makeLogOutResponse.UserName.Should().Be(userMakeLogInRequest.UserName);

        // Act
        response = await client.PostAsync("/LogIns", adminMakeLogInRequestContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        makeLogInResponse = JsonSerializer.Deserialize<MakeLogInResponse>(responseContent);
        Assert.NotNull(makeLogInResponse);
        Assert.NotNull(makeLogInResponse.UserName);
        makeLogInResponse.UserName.Should().Be(adminMakeLogInRequest.UserName);

        // Act
        response = await client.PostAsync("/LogOuts", emptyContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        makeLogOutResponse = JsonSerializer.Deserialize<MakeLogOutResponse>(responseContent);
        Assert.NotNull(makeLogOutResponse);
        Assert.NotNull(makeLogOutResponse.UserName);
        makeLogOutResponse.UserName.Should().Be(adminMakeLogInRequest.UserName);
    }
}
