namespace CatFactLogger.Services;

public interface ICatFactService
{
    Task<(string RawJson, string Fact)> GetCatFactAsync();
}
