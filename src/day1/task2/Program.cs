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
    // TODO: This should be faster when corrected.
    //var intermediateCount = counter.Count + increment;
    //zeroCount += Math.Abs(intermediateCount / 100);

    //if (counter.Count != 0 && intermediateCount < 0)
    //{
    //    zeroCount++;
    //}

    //counter.Increment(increment);

    if (increment == 0)
    {
        //Console.WriteLine("Skipping zero increment");
        continue;
    }

    var remainingIncrement = increment;
    var originalCount = counter.Count;

    if (increment > 0)
    {
        while (remainingIncrement > 0)
        {
            if (remainingIncrement > 99)
            {
                remainingIncrement -= 100;
                counter.Increment(100);
                zeroCount++;

            }
            else
            {
                if (counter.Count + remainingIncrement >= 100)
                {
                    zeroCount++;
                }

                counter.Increment(remainingIncrement);
                remainingIncrement = 0;
            }
        }
    }
    else
    {
        while (remainingIncrement < 0)
        {
            if (remainingIncrement < -99)
            {
                remainingIncrement += 100;
                counter.Increment(-100);
                zeroCount++;
            }
            else
            {
                if (counter.Count != 0 && counter.Count + remainingIncrement <= 0)
                {
                    zeroCount++;
                }

                counter.Increment(remainingIncrement);

                remainingIncrement = 0;
            }
        }
    }

    //Console.WriteLine($"{originalCount} + {increment} = {counter.Count} | {zeroCount}");
}

Console.WriteLine(zeroCount);

static string GetInputFilePath(string inputFileName, [System.Runtime.CompilerServices.CallerFilePath] string? sourceCodePath = null)
{
    var sourceCodeDirectory = Path.GetDirectoryName(sourceCodePath);
    return Path.Combine(sourceCodeDirectory!, "input", inputFileName);
}