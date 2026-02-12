using OpenQA.Selenium;

public class ScreenshotService
{
    private readonly string _root;

    public ScreenshotService(string? relativeSubPath = null)
    {
        var baseDir = AppContext.BaseDirectory;

        var subPath = string.IsNullOrWhiteSpace(relativeSubPath)
            ? Path.Combine("logs", "Screenshots")
            : relativeSubPath;

        _root = Path.GetFullPath(Path.Combine(baseDir, subPath));
        Directory.CreateDirectory(_root);
    }

    public string Save(IWebDriver driver, string testName, string label = "screenshot")
    {
        var date = DateTime.Now.ToString("yyyy-MM-dd");
        var time = DateTime.Now.ToString("HHmmssfff");

        var safeTest = MakeSafe(testName);
        var safeLabel = MakeSafe(label);

        var folder = Path.Combine(_root, date, safeTest);
        Directory.CreateDirectory(folder);

        var path = Path.Combine(folder, $"{safeLabel}_{time}.png");
        ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(path);

        Console.WriteLine($"[Screenshot] saved: {path}");
        return path;
    }

    private static string MakeSafe(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "unnamed";
        foreach (var c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        return name;
    }
}