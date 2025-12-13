using Microsoft.VisualBasic;

var redTiles = new LinkedList<Position>();
var floor = new Floor();

await foreach (var line in File.ReadLinesAsync(GetInputFilePath(
    "sinput.txt"
    //"input.txt"
)))
{
    var parts = line.Split(',');
    
    var redTile = new Position(
        long.Parse(parts[0]),
        long.Parse(parts[1])
    );

    floor.Place(new(redTile.X, redTile.Y));
    redTiles.AddLast(redTile);
}

floor.FillTheBlanks();

// Find largest filled
var incompleteRectangles = new HashSet<Rectangle>();
Rectangle? largestRectangle = null;

for (var i = redTiles.Count * redTiles.Count; i > 0; i++)
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

    bool isRectangleFullyColored =
        floor[largestRectangle.Corner1]
        && floor[largestRectangle.Corner2]
        && floor[largestRectangle.Corner3]
        && floor[largestRectangle.Corner4];

    if (!isRectangleFullyColored)
    {
        incompleteRectangles.Add(largestRectangle);
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

/// <summary>
///   X ----->
/// Y C1 .. C2
/// |  .      .
/// V C4 .. C3
/// </summary>
class Rectangle
{
    public Position Corner1 { get; }

    public Position Corner2 { get; }

    public Position Corner3 { get; }

    public Position Corner4 { get; }

    public Rectangle(Position cornerA, Position cornerB)
    {
        //   X -------->
        // Y C1:A . C2:O
        // | .         .
        // V C4:O . C3:B
        //
        // OR
        //
        // Y X -------------->
        // V C1=C4:A . C2=C3:B
        if (cornerA.X < cornerB.X && cornerA.Y <= cornerB.Y)
        {
            Corner1 = cornerA;
            Corner2 = new(cornerB.X, cornerA.Y);
            Corner3 = cornerB;
            Corner4 = new(cornerA.X, cornerB.Y);
            return;
        }

        //   X -------->
        // Y C1:O . C2:A
        // | .         .
        // V C4:B . C3:O
        //
        // OR
        //
        //   X----->
        // Y C1=C2:A
        // | .
        // V C4=C3:B
        if (cornerA.X >= cornerB.X && cornerA.Y < cornerB.Y)
        {
            Corner1 = new(cornerB.X, cornerA.Y);
            Corner2 = cornerA;
            Corner3 = new(cornerA.X, cornerB.Y);
            Corner4 = cornerB;
            return;
        }

        //   X -------->
        // Y C1:B . C2:O
        // | .         .
        // V C4:O . C3:A
        //
        // OR
        //
        // Y X -------------->
        // V C1=C4:B . C2=C3:A
        if (cornerA.X > cornerB.X && cornerA.Y >= cornerB.Y)
        {
            Corner1 = cornerB;
            Corner2 = new(cornerA.X, cornerB.Y);
            Corner3 = cornerA;
            Corner4 = new(cornerB.X, cornerA.Y);
            return;
        }

        //   X -------->
        // Y C1:O . C2:B
        // | .         .
        // V C4:A . C3:O
        //
        // OR
        //
        //   X----->
        // Y C1=C2:B
        // | .
        // V C4=C3:A
        if (cornerA.X <= cornerB.X && cornerA.Y > cornerB.Y)
        {
            Corner1 = new(cornerA.X, cornerB.Y);
            Corner2 = cornerB;
            Corner3 = new(cornerB.X, cornerA.Y);
            Corner4 = cornerA;
            return;
        }

        throw new InvalidOperationException();
    }

    public long Area => (Corner3.X - Corner1.X + 1) * (Corner3.Y - Corner1.Y + 1);

    public override bool Equals(object? obj)
    {
        return obj is Rectangle other && this.Corner1.Equals(other.Corner1) && this.Corner2.Equals(other.Corner2);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Corner1.GetHashCode(), this.Corner2.GetHashCode());
    }
}

record Position(long X, long Y)
{
    public override int GetHashCode() => HashCode.Combine(X, Y);
}

class Row
{
    public Position First { get; private set; }

    public Position Last { get; private set; }

    public Row(Position first, Position last)
    {
        if (first.Y != last.Y)
        {
            throw new InvalidOperationException();
        }

        First = first;
        Last = last;
    }

    public bool IsColored(long x) => x >= this.First.X && x <= this.Last.X;

    public bool Place(Position position)
    {
        if (position.Y != this.First.Y)
        {
            throw new InvalidOperationException();
        }

        if (position.X < this.First.X)
        {
            this.First = position;
            return true;
        }

        if (position.X > this.Last.X)
        {
            this.Last = position;
            return true;
        }

        return false;
    }
}

class Column
{
    public Position First { get; private set; }

    public Position Last { get; private set; }

    public Column(Position first, Position last)
    {
        if (first.X != last.X)
        {
            throw new InvalidOperationException();
        }

        First = first;
        Last = last;
    }

    public bool IsColored(long y) => y >= this.First.Y && y <= this.Last.Y;

    public bool Place(Position position)
    {
        if (position.X != this.First.X)
        {
            throw new InvalidOperationException();
        }

        if (position.Y < this.First.Y)
        {
            this.First = position;
            return true;
        }
        
        if (position.Y > this.Last.Y)
        {
            this.Last = position;
            return true;
        }

        return false;
    }
}

class Floor
{
    private readonly Dictionary<long, Row> _rows = new();

    private readonly Dictionary<long, Column> _columns = new();

    public bool this[Position position] =>
        (_rows.TryGetValue(position.Y, out var row) && row.IsColored(position.X))
        || (_columns.TryGetValue(position.X, out var column) && column.IsColored(position.Y));

    public bool Place(long x, long y) => this.Place(new(x, y));

    public bool Place(Position position)
    {
        bool anyChange = false;

        if (_rows.TryGetValue(position.Y, out var row))
        {
            anyChange |= row.Place(position);
        }
        else
        {
            _rows[position.Y] = new(position, position);
            anyChange = true;
        }

        if (_columns.TryGetValue(position.X, out var column))
        {
            anyChange |= column.Place(position);
        }
        else
        {
            _columns[position.X] = new(position, position);
            anyChange = true;
        }

        return anyChange;
    }

    public void FillTheBlanks()
    {
        var iterations = 0L;
        bool anyChange;

        do
        {
            anyChange = false;

            foreach (var row in _rows.Values.ToArray())
            {
                foreach (var column in _columns.Values.ToArray())
                {
                    anyChange |= this.Place(row.First.X, column.First.Y);
                    anyChange |= this.Place(row.First.X, column.Last.Y);
                    anyChange |= this.Place(row.Last.X, column.First.Y);
                    anyChange |= this.Place(row.Last.X, column.Last.Y);
                }
            }

            Console.WriteLine(iterations++);
        } while (anyChange);
    }
}