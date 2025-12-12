using Microsoft.VisualBasic;

var redTiles = new LinkedList<Tile>();
var floor = new Floor();

await foreach (var line in File.ReadLinesAsync(GetInputFilePath(
    //"sinput.txt"
    "input.txt"
)))
{
    var parts = line.Split(',');
    
    var tile = new Tile(
        long.Parse(parts[0]),
        long.Parse(parts[1]),
        TileColor.Red
    );

    floor.Place(tile);
    redTiles.AddLast(tile);
}

// Fill lines
for (var y = 0L; y < floor.Height; y++)
{
    Tile? firstRedTile = null;
    Tile? secondRedTile = null;

    for (var x = 0L; x < floor.Width; x++)
    {
        var currentTile = floor[x, y];

        if (currentTile == null || currentTile.Color != TileColor.Red)
        {
            continue;
        }

        if (firstRedTile == null)
        {
            firstRedTile = currentTile;
        }
        else
        {
            secondRedTile = currentTile;
            break;
        }
    }

    if (firstRedTile == null || secondRedTile == null)
    {
        continue;
    }

    for (var x = firstRedTile.X + 1; x < secondRedTile.X; x++)
    {
        floor.Place(new Tile(x, y, TileColor.Green));
    }
}

// Fill columns
for (var x = 0L; x < floor.Width; x++)
{
    Tile? firstRedTile = null;
    Tile? secondRedTile = null;

    for (var y = 0L; y < floor.Height; y++)
    {
        var currentTile = floor[x, y];

        if (currentTile == null || currentTile.Color != TileColor.Red)
        {
            continue;
        }

        if (firstRedTile == null)
        {
            firstRedTile = currentTile;
        }
        else
        {
            secondRedTile = currentTile;
            break;
        }
    }

    if (firstRedTile == null || secondRedTile == null)
    {
        continue;
    }

    for (var y = firstRedTile.Y + 1; y < secondRedTile.Y; y++)
    {
        floor.Place(new(x, y, TileColor.Green));
    }
}

// Fill area
for (var y = 0L; y < floor.Height; y++)
{
    Tile? firstColoredTile = null;
    Tile? lastColoredTile = null;

    for (var x = 0L; x < floor.Width; x++)
    {
        var currentTile = floor[x, y];

        if (currentTile != null && currentTile.Color != TileColor.Other)
        {
            firstColoredTile = currentTile;
            break;
        }
    }

    if (firstColoredTile == null)
    {
        continue;
    }

    for (var x = floor.Width - 1; x > firstColoredTile.X; x--)
    {
        var currentTile = floor[x, y];

        if (currentTile != null && currentTile.Color != TileColor.Other)
        {
            lastColoredTile = currentTile;
            break;
        }
    }

    if (lastColoredTile == null)
    {
        continue;
    }

    for (var x = firstColoredTile.X + 1; x < lastColoredTile.X; x++)
    {
        if (floor[x, y] == null)
        {
            floor.Place(new(x, y, TileColor.Green));
        }
    }
}

// Print
for (var y = 0L; y < floor.Height; y++)
{
    for (var x = 0L; x < floor.Width; x++)
    {
        Console.Write(floor[x, y] switch
        {
            { Color: TileColor.Red } => '#',
            { Color: TileColor.Green } => 'X',
            _ => '.'
        });
    }

    Console.WriteLine();
}

// Find largest filled
var incompleteRectangles = new HashSet<Rectangle>();
Rectangle? largestRectangle = null;

for (var i = floor.Width * floor.Height; i > 0; i++)
{
    largestRectangle = null;

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
                if (incompleteRectangles.Contains(rectangle))
                {
                    continue;
                }

                Console.WriteLine($"Candidate: {rectangle.Area}");
                largestRectangle = rectangle;
            }
        }
    }

    if (largestRectangle == null)
    {
        throw new InvalidOperationException();
    }

    Console.WriteLine();

    for (var y = 0L; y < floor.Height; y++)
    {
        for (var x = 0L; x < floor.Width; x++)
        {
            var tile = floor[x, y];

            Console.Write(tile switch
            {
                _ when tile == largestRectangle.Corner1 || tile == largestRectangle.Corner2 => 'O',
                { Color: TileColor.Red } => '#',
                { Color: TileColor.Green } => 'X',
                _ => '.'
            });
        }

        Console.WriteLine();
    }

    bool isRectangleFullyColored = true;

    var x1 = largestRectangle.Corner1.X;
    var y1 = largestRectangle.Corner1.Y;
    var x2 = largestRectangle.Corner2.X;
    var y2 = largestRectangle.Corner2.Y;

    if (x1 > x2)
    {
        var tmp = x2;
        x2 = x1;
        x1 = tmp;
    }

    if (y1 > y2)
    {
        var tmp = y2;
        y2 = y1;
        y1 = tmp;
    }

    for (var y = y1; y < y2; y++)
    {
        for (var x = x1; x < x2; x++)
        {
            var currentTile = floor[x, y];

            if (currentTile == null || currentTile.Color == TileColor.Other)
            {
                isRectangleFullyColored = false;
                incompleteRectangles.Add(largestRectangle);
                break;
            }
        }

        if (!isRectangleFullyColored)
        {
            break;
        }
    }

    if (!isRectangleFullyColored)
    {
        continue;
    }

    break;
}

Console.WriteLine(largestRectangle!.Area);

static string GetInputFilePath(string inputFileName, [System.Runtime.CompilerServices.CallerFilePath] string? sourceCodePath = null)
{
    var sourceCodeDirectory = Path.GetDirectoryName(sourceCodePath);
    return Path.Combine(sourceCodeDirectory!, "input", inputFileName);
}

enum TileColor
{
    Red,
    Green,
    Other
}

class Tile
{
    public long X { get; }

    public long Y { get; }

    public TileColor Color { get; set; }

    public Tile(long x, long y, TileColor color)
    {
        X = x;
        Y = y;
        Color = color;
    }

    public override bool Equals(object? obj)
    {
        return obj is Tile other && this.X == other.X && this.Y == other.Y && this.Color == other.Color;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.X, this.Y, this.Color);
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

    public override bool Equals(object? obj)
    {
        return obj is Rectangle other && this.Corner1.Equals(other.Corner1) && this.Corner2.Equals(other.Corner2);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Corner1.GetHashCode(), this.Corner2.GetHashCode());
    }
}

class Floor
{
    private readonly Dictionary<(long X, long Y), Tile?> _tiles = new();

    public long Width { get; private set; }

    public long Height { get; private set; }

    public Tile? this[long x, long y] => _tiles.TryGetValue((x, y), out var tile) ? tile : null;

    public void Place(Tile tile)
    {
        _tiles[(tile.X, tile.Y)] = tile;
        Width = Math.Max(Width, tile.X + 1);
        Height = Math.Max(Height, tile.Y + 1);
    }
}