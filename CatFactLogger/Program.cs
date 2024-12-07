using CatFactLogger;
using CatFactLogger.Helpers;
using CatFactLogger.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceProvider = BuildServiceProvider(args);

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
var application = serviceProvider.GetRequiredService<Application>();

try
{
    await application.Run();
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred while running the application");
}

static ServiceProvider BuildServiceProvider(string[] args)
{
    var filePath = ArgumentParser.GetFilePath(args);

    return new ServiceCollection()
        .AddHttpClient<ICatFactService, CatFactService>()
        .Services
        .AddSingleton<IFileService, FileService>()
        .AddSingleton(services =>
        {
            var catFactService = services.GetRequiredService<ICatFactService>();
            var fileService = services.GetRequiredService<IFileService>();
            return new Application(catFactService, fileService, filePath);
        })
        .AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Error);
        })
        .BuildServiceProvider();
}