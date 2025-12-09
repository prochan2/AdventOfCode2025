var redTiles = new LinkedList<Tile>();

await foreach (var line in File.ReadLinesAsync(GetInputFilePath(
    //"sinput.txt"
    "input.txt"
)))
{
    var parts = line.Split(',');
    
    var tile = new Tile(
        long.Parse(parts[0]),
        long.Parse(parts[1])
    );

    redTiles.AddLast(tile);
}

Rectangle? largestRectangle = null;

foreach (var corner1 in redTiles)
{
    foreach (var corner2 in redTiles)
    {
        if (corner1 == corner2)
        {
            continue;
        }

        var rectangle = new Rectangle(corner1, corner2);

        if (largestRectangle == null || rectangle.Area > largestRectangle.Area)
        {
            largestRectangle = rectangle;
        }
    }
}

Console.WriteLine(largestRectangle!.Area);

static string GetInputFilePath(string inputFileName, [System.Runtime.CompilerServices.CallerFilePath] string? sourceCodePath = null)
{
    var sourceCodeDirectory = Path.GetDirectoryName(sourceCodePath);
    return Path.Combine(sourceCodeDirectory!, "input", inputFileName);
}

class Tile
{
    public long X { get; }

    public long Y { get; }

    public Tile(long x, long y)
    {
        X = x;
        Y = y;
    }
}

class Rectangle
{
    public Tile Corner1 { get; }

    public Tile Corner2 { get; }

    public Rectangle(Tile corner1, Tile corner2)
    {
        Corner1 = corner1;
        Corner2 = corner2;
    }

    public long Area => Math.Abs((Corner2.X - Corner1.X + 1) * (Corner2.Y - Corner1.Y + 1));
}