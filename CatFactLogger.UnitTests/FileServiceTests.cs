using CatFactLogger.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.IO.Abstractions;

namespace CatFactLogger.UnitTests;

public class FileServiceTests
{
    private readonly ILogger<FileService> _logger;
    private readonly IFileSystem _fileSystem;
    private readonly FileService _fileService;

    public FileServiceTests()
    {
        _logger = Substitute.For<ILogger<FileService>>();
        _fileSystem = Substitute.For<IFileSystem>();
        _fileService = new FileService(_logger, _fileSystem);
    }

    [Fact]
    public void InitializeFile_FileExists_DoesNotCreateNewFile()
    {
        // Arrange
        var filePath = "filePath.txt";
        _fileSystem.File.Exists(filePath).Returns(true);

        // Act
        _fileService.InitializeFile(filePath);

        // Assert
        _fileSystem.File.DidNotReceive().Create(Arg.Any<string>());
    }

    [Fact]
    public void InitializeFile_CreatingFileThrowsException_ThrowsException()
    {
        // Arrange
        var filePath = "filePath.txt";
        _fileSystem.File.Exists(filePath).Returns(false);
        _fileSystem.File.Create(filePath).Throws<DirectoryNotFoundException>();

        // Act
        Action act = () => _fileService.InitializeFile(filePath);

        // Assert
        act.Should().Throw<DirectoryNotFoundException>();
    }

    [Fact]
    public void SaveContent_AccessingFileThrowsException_ThrowsException()
    {
        // Arrange
        var filePath = "filePath.txt";
        var fileContent = "Content in valid JSON format";
        _fileSystem.File
            .When(x => x.AppendAllText(filePath, fileContent + Environment.NewLine))
            .Do(x => throw new IOException());

        // Act
        Action act = () => _fileService.SaveContent(filePath, fileContent);

        // Assert
        act.Should().Throw<IOException>();
    }

    [Fact]
    public void SaveContent_SuccessfulWriteToFile_DoesNotThrowAnyException()
    {
        // Arrange
        var filePath = "filePath.txt";
        var fileContent = "Content in valid JSON format";
        _fileSystem.File.AppendAllText(filePath, fileContent + Environment.NewLine);

        // Act
        Action act = () => _fileService.SaveContent(filePath, fileContent);

        // Assert
        act.Should().NotThrow();
    }
}
