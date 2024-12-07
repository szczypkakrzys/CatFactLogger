using CatFactLogger.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http.Json;

namespace CatFactLogger.UnitTests;

public class CatServiceTests
{
    private readonly HttpClient _httpClient;
    private readonly MockHttpMessageHandler _httpMock;
    private readonly ILogger<CatFactService> _logger;
    private readonly CatFactService _catFactService;
    private readonly string ApiUrl = "https://catfact.ninja/fact";

    public CatServiceTests()
    {
        _httpMock = new MockHttpMessageHandler();
        _httpClient = _httpMock.ToHttpClient();
        _logger = Substitute.For<ILogger<CatFactService>>();
        _catFactService = new CatFactService(_httpClient, _logger);
    }

    [Fact]
    public async Task GetCatFactAsync_SuccessApiResponse_ReturnsFactAndResponseJsonAsync()
    {
        // Arrange
        var expectedFact = "Baking chocolate is the most dangerous chocolate to your cat.";
        var jsonContent = JsonContent.Create(new
        {
            fact = expectedFact,
            length = 61
        });
        var expectedJson = await jsonContent.ReadAsStringAsync();

        _httpMock.When(ApiUrl).Respond(HttpStatusCode.OK, jsonContent);

        // Act
        var (rawJson, fact) = await _catFactService.GetCatFactAsync();

        // Assert
        rawJson.Should().Be(expectedJson);
        fact.Should().Be(expectedFact);
    }

    [Fact]
    public async Task GetCatFactAsync_UnauthorizedApiResponse_ThrowsHttpRequestException()
    {
        // Arrange
        _httpMock.When(ApiUrl).Respond(HttpStatusCode.Unauthorized);

        // Act
        Func<Task> act = _catFactService.GetCatFactAsync;

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task GetCatFactAsync_InvalidJsonResponseFormat_ThrowsInvalidOperationExceptionAsync()
    {
        // Arrange
        var invalidJson = "{ \"fact\": \"Baking chocolate is the most dangerous chocolate to your cat.\", \"length\": 61";
        _httpMock.When(ApiUrl).Respond("application/json", invalidJson);

        // Act
        Func<Task> act = _catFactService.GetCatFactAsync;

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Error occured while processing the response from the Cat Fact API.");
    }

    [Fact]
    public async Task GetCatFactAsync_MissingCatFact_ReturnsDefaultFactValueAsync()
    {
        // Arrange
        var jsonContent = JsonContent.Create(new
        {
            length = 61
        });
        var expectedJson = await jsonContent.ReadAsStringAsync();

        _httpMock.When(ApiUrl).Respond(HttpStatusCode.OK, jsonContent);

        // Act
        var (rawJson, fact) = await _catFactService.GetCatFactAsync();

        // Assert
        rawJson.Should().Be(expectedJson);
        fact.Should().Be("No fact available");
    }
}
