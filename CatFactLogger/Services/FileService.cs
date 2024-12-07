using Microsoft.Extensions.Logging;

namespace CatFactLogger.Services;

public class FileService : IFileService
{
    private readonly ILogger<FileService> _logger;

    public FileService(ILogger<FileService> logger)
    {
        _logger = logger;
    }

    public void InitializeFile(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
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
            File.AppendAllText(filePath, content + Environment.NewLine);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured while writing to the file: '{FilePath}'", filePath);
            throw;
        }
    }
}
