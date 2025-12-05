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

var freshSum = intervals.Aggregate(0L, (sum, interval) => sum + interval switch
{
    null => 0,
    _ => interval.End - interval.Start + 1
});

Console.WriteLine(freshSum);

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