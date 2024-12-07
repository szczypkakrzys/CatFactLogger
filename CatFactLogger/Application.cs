using CatFactLogger.Services;

namespace CatFactLogger;

public class Application
{
    private readonly ICatFactService _catFactService;
    private readonly IFileService _fileService;
    private readonly string _filePath;
    private readonly string _keyword = "cat";

    public Application(
        ICatFactService catFactService,
        IFileService fileService,
        string filePath)
    {
        _catFactService = catFactService;
        _fileService = fileService;
        _filePath = filePath;
    }

    public async Task Run()
    {
        _fileService.InitializeFile(_filePath);

        Console.WriteLine($"Hi!\nYour Cat Facts will be stored in file: {_filePath}");

        var stopRequested = false;
        while (!stopRequested)
        {
            await ProcessCatFact();

            if (!UserWantsMoreFacts())
                stopRequested = true;
        }

        Console.WriteLine("Come back for more Cat Fact :)");
    }

    private async Task ProcessCatFact()
    {
        var (rawJson, catFact) = await _catFactService.GetCatFactAsync();

        _fileService.SaveContent(_filePath, rawJson);

        Console.WriteLine($"Cat Fact: {catFact}");
    }

    private bool UserWantsMoreFacts()
    {
        Console.Write($@"For more cat facts, type ""{_keyword}"": ");
        var input = Console.ReadLine();
        return input?.Trim() == _keyword;
    }
}
