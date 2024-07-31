using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PointService;

internal class Program
{
    private static void Main(string[] args)
    {
        var fileName = "points.txt";
        var directory = Path.Combine(AppContext.BaseDirectory, "Resources");
        var filePath = Path.Combine(directory, fileName);

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        var pointManager = new PointManager();

        try
        {
            using var reader = new StreamReader(filePath);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var point = line
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToArray();

                if (point.Length != 2)
                {
                    logger.LogInformation(Constants.InvalidPoint, line);
                    continue;
                }

                var x = TryParseNullable(point[0]);
                var y = TryParseNullable(point[1]);
                if (x is null || y is null)
                {
                    logger.LogInformation(Constants.InvalidPoint, line);
                    continue;
                }

                pointManager.AddPoint(x.Value, y.Value);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, Constants.ExeptionMessage);
            return;
        }

        var furthestPoints = pointManager.GetFurthestPointsOrDefault();
        Console.WriteLine(Constants.OutputTitle);
        if (furthestPoints is null || !furthestPoints.Any())
        {
            Console.WriteLine(Constants.NoDataTitle);
            return;
        }

        foreach (var point in furthestPoints)
        {
            Console.WriteLine(string.Format(Constants.OutputTemplate, point.X, point.Y, point.Quadrant));
        }
    }

    static void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(configure => configure.AddConsole())
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
    }

    static double? TryParseNullable(string s)
    {
        return double.TryParse(s, out double value) ? value : null;
    }
}
