namespace CatFactLogger.Helpers;

public static class ArgumentParser
{
    public static string GetFilePath(string[] args)
    {
        const string defaultFileName = "CatFacts.txt";
        var filePath = args.FirstOrDefault(arg => arg.StartsWith("-path="))?.Split('=')[1];

        if (string.IsNullOrWhiteSpace(filePath))
        {
            return Path.Combine(Directory.GetCurrentDirectory(), defaultFileName);
        }

        if (!filePath.EndsWith(".txt"))
        {
            Console.WriteLine($"Warning: Provided path '{filePath}' is not a .txt file. Using default path.");
            return Path.Combine(Directory.GetCurrentDirectory(), defaultFileName);
        }

        return filePath;
    }
}