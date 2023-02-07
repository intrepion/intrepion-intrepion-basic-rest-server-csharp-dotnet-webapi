using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using IntrepionApp.WebApi.HealthCheck;

namespace IntrepionApp.Tests.WebApi.HealthCheck;

public class TestHealthCheckController
{
    [Fact]
    public void Get_Returns200()
    {
        // Arrange
        var controller = new HealthCheckController();

        // Act
        var actualResult = controller.Get();

        // Assert
        actualResult.Should().BeOfType<OkObjectResult>();
        var okObjectResult = (OkObjectResult)actualResult;
        okObjectResult.StatusCode.Should().Be(200);
    }
}
