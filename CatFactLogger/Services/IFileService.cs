namespace CatFactLogger.Services;

public interface IFileService
{
    void InitializeFile(string filePath);
    void SaveContent(string filePath, string content);
}
