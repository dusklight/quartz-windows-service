using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QuartzWindowsServiceApp.HelloWorld;

namespace QuartzWindowsServiceApp.Test.HelloWorld;

/// <summary>
/// Mainly for demonstrating how to test with IOptions<T>.
/// </summary>
public class HelloWorldServiceTest
{
    [Fact]
    public void GetMessage_ReturnsCorrectMessage()
    {
        // Arrange
        const string messageText = "Testing Hello World!";
        var loggerMock = new Mock<ILogger<HelloWorldService>>();
        var options = Options.Create(new HelloWorldServiceOptions { MessageText = messageText });
        var service = new HelloWorldService(loggerMock.Object, options);

        // Act
        var message = service.GetMessage();

        // Assert
        message.Should().Be(messageText);
    }

    [Fact]
    public void GetMessage_ReturnsEmptyString_WhenValueIsNull()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<HelloWorldService>>();
        var options = Options.Create(new HelloWorldServiceOptions { MessageText = null });
        var service = new HelloWorldService(loggerMock.Object, options);

        // Act
        var message = service.GetMessage();

        // Assert
        message.Should().BeEmpty();
    }
}
