var file = File.OpenRead(GetInputFilePath(
    //"sinput.txt"
    "input.txt"
));

int c;

var beams = new List<long>();

while ((c = file.ReadByte()) >= 0)
{
    if (c == '\n')
    {
        break;
    }

    if (c == '\r')
    {
        continue;
    }

    Console.Write((char)c);

    beams.Add(c == 'S' ? 1L : 0L);
}

Console.WriteLine();

var splitCount = 1L;
var beamIndex = 0;
var nextBeams = beams.ToList();
nextBeams[beamIndex] = 0L;
var splitters = beams.Select(_ => false).ToList(); // For tracing only

while ((c = file.ReadByte()) >= 0)
{
    if (c == '\n')
    {
        for (var i = 0; i < nextBeams.Count; i++)
        {
            if (splitters[i])
            {
                Console.Write('X');
                Console.Write(nextBeams[i].ToString("D2"));
            }
            else if (nextBeams[i] > 0L)
            {
                Console.Write(nextBeams[i].ToString("D3"));
            }
            else
            {
                Console.Write("   ");
            }

            Console.Write(' ');
        }

        Console.WriteLine(splitCount);

        beamIndex = 0;
        var tempBeams = nextBeams;
        nextBeams = beams;
        nextBeams[beamIndex] = 0L;
        beams = tempBeams;
        continue;
    }

    if (c == '^')
    {
        splitters[beamIndex] = true;

        if (beams[beamIndex] > 0L)
        {
            splitCount += beams[beamIndex];

            if (beamIndex > 0)
            {
                nextBeams[beamIndex - 1] += beams[beamIndex];
            }

            // There are newver two splitters next to each other.
            nextBeams[beamIndex] = 0L;

            if (beamIndex < nextBeams.Count - 1)
            {
                nextBeams[beamIndex + 1] = beams[beamIndex];
            }
        }

        beamIndex++;
    }

    if (c == '.')
    {
        splitters[beamIndex] = false;
        nextBeams[beamIndex] += beams[beamIndex];

        if (beamIndex < nextBeams.Count - 1)
        {
            nextBeams[beamIndex + 1] = 0L;
        }

        beamIndex++;
    }
}

for (var i = 0; i < nextBeams.Count; i++)
{
    if (splitters[i])
    {
        Console.Write('X');
        Console.Write(nextBeams[i].ToString("D2"));
    }
    else if (nextBeams[i] > 0L)
    {
        Console.Write(nextBeams[i].ToString("D3"));
    }
    else
    {
        Console.Write("   ");
    }

    Console.Write(' ');
}

Console.WriteLine(splitCount);

// 1208275172079390 Too high.
// 1208275172079389 Too high.
Console.WriteLine(splitCount);

static string GetInputFilePath(string inputFileName, [System.Runtime.CompilerServices.CallerFilePath] string? sourceCodePath = null)
{
    var sourceCodeDirectory = Path.GetDirectoryName(sourceCodePath);
    return Path.Combine(sourceCodeDirectory!, "input", inputFileName);
}