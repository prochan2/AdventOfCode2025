var lines = File.ReadLines(GetInputFilePath(
    //"sinput.txt"
    "input.txt"
));

var increments = lines.Select(line =>
{
    var multiplier = line[0] switch
    {
        'L' => -1,
        'R' => 1,
        _ => throw new InvalidOperationException($"Unexpected value '{line[0]}'")
    };

    var value = long.Parse(line[1..]);

    return multiplier * value;
});

var counter = new RotaryCounter(50);
var zeroCount = counter.Count == 0 ? 1L : 0L;

foreach (var increment in increments)
{
    counter.Increment(increment);

    if (counter.Count == 0)
    {
        zeroCount++;
    }
}

Console.WriteLine(zeroCount);

static string GetInputFilePath(string inputFileName, [System.Runtime.CompilerServices.CallerFilePath] string? sourceCodePath = null)
{
    var sourceCodeDirectory = Path.GetDirectoryName(sourceCodePath);
    return Path.Combine(sourceCodeDirectory!, "input", inputFileName);
}