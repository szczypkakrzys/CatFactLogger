using CatFactLogger.Helpers;
using FluentAssertions;

namespace CatFactLogger.UnitTests;

public class ArgumentParserTests
{
    private readonly string _defaultPath = Path.Combine(Directory.GetCurrentDirectory(), "CatFacts.txt");

    [Fact]
    public void GetFilePath_ValidPath_ReturnsCorrectFilePath()
    {
        // Arrange
        var inputPath = "C:\\test\\file.txt";
        var args = new[] { $"-path={inputPath}" };

        // Act
        var result = ArgumentParser.GetFilePath(args);

        // Assert
        result.Should().Be(inputPath);
    }

    [Fact]
    public void GetFilePath_InvalidPathExtension_ReturnsDefaultPath()
    {
        // Arrange
        var args = new[] { "-path=C:\\test\\file.jpg" };

        // Act
        var result = ArgumentParser.GetFilePath(args);

        // Assert
        result.Should().Be(_defaultPath);
    }

    [Fact]
    public void GetFilePath_NoPathArgument_ReturnsDefaultPath()
    {
        // Arrange
        var args = Array.Empty<string>();

        // Act
        var result = ArgumentParser.GetFilePath(args);

        // Assert
        result.Should().Be(_defaultPath);
    }

    [Fact]
    public void GetFilePath_EmptyPathArgument_ReturnsDefaultPath()
    {
        // Arrange
        var args = new[] { "-path=" };

        // Act
        var result = ArgumentParser.GetFilePath(args);

        // Assert
        result.Should().Be(_defaultPath);
    }
}
