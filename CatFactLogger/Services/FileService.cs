using Microsoft.Extensions.Logging;
using System.IO.Abstractions;

namespace CatFactLogger.Services;

public class FileService : IFileService
{
    private readonly ILogger<FileService> _logger;
    private readonly IFileSystem _fileSystem;

    public FileService(
        ILogger<FileService> logger,
        IFileSystem fileSystem)
    {
        _logger = logger;
        _fileSystem = fileSystem;
    }

    public void InitializeFile(string filePath)
    {
        try
        {
            if (!_fileSystem.File.Exists(filePath))
            {
                _fileSystem.File.Create(filePath).Dispose();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured while initializing the file: '{FilePath}'", filePath);
            throw;
        }
    }

    public void SaveContent(string filePath, string content)
    {
        try
        {
            _fileSystem.File.AppendAllText(filePath, content + Environment.NewLine);
        }
        catch (IOException ex)
        {
            _logger.LogError(ex, "Error occured while writing to the file: '{FilePath}'", filePath);
            throw;
        }
    }
}
