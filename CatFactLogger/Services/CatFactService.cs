
using CatFactLogger.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CatFactLogger.Services;

public class CatFactService : ICatFactService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CatFactService> _logger;
    private readonly string ApiUrl = "https://catfact.ninja/fact";

    public CatFactService(
        HttpClient httpClient,
        ILogger<CatFactService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<(string RawJson, string Fact)> GetCatFactAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(ApiUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var fact = RetrieveFactFromJson(content);

            return (content, fact);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error while making an HTTP request to {Url}.", ApiUrl);
            throw;
        }
    }

    private string RetrieveFactFromJson(string content)
    {
        try
        {
            var catFactResponse = JsonSerializer.Deserialize<CatFact>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return catFactResponse?.Fact ?? "No fact available.";
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize the cat fact JSON.");
            throw new InvalidOperationException("Error processing the response from the Cat Fact API.", ex);
        }
    }
}
