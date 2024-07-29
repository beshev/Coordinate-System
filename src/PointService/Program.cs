using PointService;

var filePath = $"{AppContext.BaseDirectory}/Resources/points.txt";

var pointManager = new PointManager();

using var reader = new StreamReader(filePath);
var line = reader.ReadLine();
while (line != null)
{
    // TODO: Input validation
    var point = line
        .Trim()
        .Split(",", StringSplitOptions.RemoveEmptyEntries)
        .ToArray();

    if (point.Length <= 1)
    {
        // TODO: skiping only this point or handle it as a exeption;
        line = reader.ReadLine();
        continue;
    }

    // TODO: What type must X and Y be.
    var x = TryParseNullable(point[0]);
    var y = TryParseNullable(point[1]);

    if(x is null || y is null)
    {
        // TODO: skiping only this point or handle it as a exeption;
        line = reader.ReadLine();
        continue;
    }

    pointManager.AddPoint(x.Value, y.Value);
    line = reader.ReadLine();
}

foreach (var point in pointManager.GetFurthestPointsOrDefault() ?? [])
{
    // TODO: How they like to be printed?
    Console.WriteLine($"X => {point.X}, Y => {point.Y}, Quadrant => {point.Quadrant}");
}

static double? TryParseNullable(string s)
    => double.TryParse(s, out double value) ? value : null;
