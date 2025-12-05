var lines = File.ReadLines(GetInputFilePath(
    //"sinput.txt"
    "input.txt"
));

var intervals = new List<Interval?>();

var linesEnumerator = lines.GetEnumerator();

while (true)
{
    var line = linesEnumerator.MoveNext() ? linesEnumerator.Current : throw new InvalidOperationException();

    if (line == "")
    {
        break;
    }

    var parts = line.Split('-');
    var start = long.Parse(parts[0]);
    var end = long.Parse(parts[1]);
    intervals.Add(new Interval(start, end));
}

intervals.Sort((x, y) => x!.Start.CompareTo(y!.Start));

for (int i = 0; i < intervals.Count - 1; i++)
{
    if (intervals[i]!.Overlaps(intervals[i + 1]!))
    {
        intervals[i + 1] = intervals[i]!.Merge(intervals[i + 1]!);
        intervals[i] = null;
    }
}

var freshCount = 0L;

while (linesEnumerator.MoveNext())
{
    var id = long.Parse(linesEnumerator.Current);

    bool isFresh = false;
    int left = 0;
    int right = intervals.Count - 1;

    while (left <= right)
    {
        var middleIndex = (left + right) / 2;

        Console.WriteLine($"{id}: {left}-{right} / {middleIndex}");
        
        var i = middleIndex;

        while (i < right && intervals[i] == null)
        {
            i++;
        }

        var interval = intervals[i];

        if (interval == null)
        {
            i = middleIndex - 1;

            while (i > left && intervals[i] == null)
            {
                i--;
            }

            interval = intervals[i];

            if (interval == null)
            {
                break;
            }

            right = i;
        }

        if (interval!.Contains(id))
        {
            isFresh = true;
            break;
        }

        if (id < interval.Start)
        {
            right = i - 1;
        }
        else
        {
            left = i + 1;
        }
    }

    Console.WriteLine(isFresh ? "FRESH" : "spoiled");

    if (isFresh)
    {
        freshCount++;
    }
}

Console.WriteLine(freshCount);

static string GetInputFilePath(string inputFileName, [System.Runtime.CompilerServices.CallerFilePath] string? sourceCodePath = null)
{
    var sourceCodeDirectory = Path.GetDirectoryName(sourceCodePath);
    return Path.Combine(sourceCodeDirectory!, "input", inputFileName);
}

internal class Interval
{
    public long Start { get; }

    public long End { get; }

    public Interval(long start, long end)
    {
        Start = start;
        End = end;
    }

    public bool Contains(long value)
        => this.Start <= value && value <= this.End;

    public bool Overlaps(Interval other)
        => this.Contains(other.Start)
        || this.Contains(other.End)
        || other.Contains(this.Start)
        || other.Contains(this.End)
        || (this.Start > other.Start && this.End < other.End)
        || (other.Start > this.Start && other.End < this.End);

    public Interval Merge(Interval other)
        => new Interval(Math.Min(this.Start, other.Start), Math.Max(this.End, other.End));
}