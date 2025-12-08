var file = File.OpenRead(GetInputFilePath(
    //"sinput.txt"
    "input.txt"
));

int c;

var beams = new List<bool>();

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

    beams.Add(c == 'S');
}

Console.WriteLine();

var splitCount = 0L;
var beamIndex = 0;
var nextBeams = beams.ToList();
nextBeams[beamIndex] = false;
var splitters = beams.ToList(); // For tracing only

while ((c = file.ReadByte()) >= 0)
{
    if (c == '\n')
    {
        for (var i = 0; i < nextBeams.Count; i++)
        {
            if (splitters[i] && nextBeams[i])
            {
                Console.Write('X');
            }
            else if (splitters[i])
            {
                Console.Write('^');
            }
            else if (nextBeams[i])
            {
                Console.Write('|');
            }
            else
            {
                Console.Write('.');
            }
        }

        Console.WriteLine();

        beamIndex = 0;
        var tempBeams = nextBeams;
        nextBeams = beams;
        nextBeams[beamIndex] = false;
        beams = tempBeams;
        continue;
    }

    if (c == '^')
    {
        splitters[beamIndex] = true;

        if (beams[beamIndex])
        {
            splitCount++;

            if (beamIndex > 0)
            {
                nextBeams[beamIndex - 1] = true;
            }

            nextBeams[beamIndex] = false;

            if (beamIndex < nextBeams.Count - 1)
            {
                nextBeams[beamIndex + 1] = true;
            }
        }

        beamIndex++;
    }

    if (c == '.')
    {
        splitters[beamIndex] = false;
        nextBeams[beamIndex] |= beams[beamIndex];

        if (beamIndex < nextBeams.Count - 1)
        {
            nextBeams[beamIndex + 1] = false;
        }

        beamIndex++;
    }
}

Console.WriteLine(splitCount);

static string GetInputFilePath(string inputFileName, [System.Runtime.CompilerServices.CallerFilePath] string? sourceCodePath = null)
{
    var sourceCodeDirectory = Path.GetDirectoryName(sourceCodePath);
    return Path.Combine(sourceCodeDirectory!, "input", inputFileName);
}