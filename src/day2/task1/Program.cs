using System.Numerics;

var file = File.OpenRead(GetInputFilePath(
    //"sinput.txt"
    "input.txt"
));

var sum = 0L;
var startString = new List<char>();
var endString = new List<char>();
int c;
var readingIntervalStart = true;

while ((c = file.ReadByte()) >= 0)
{
    if (readingIntervalStart)
    {
        if (c == '-')
        {
            readingIntervalStart = false;
            continue;
        }

        startString.Add((char)c);
    }
    else
    {
        if (c == ',')
        {
            AddInvalidIds();

            startString.Clear();
            endString.Clear();

            readingIntervalStart = true;
            continue;
        }

        endString.Add((char)c);
    }
}

AddInvalidIds();

long ToLong(IEnumerable<char> numberString) => numberString.Aggregate(0L, (aggregate, digitCharacter) => (aggregate * 10) + (digitCharacter - '0'));

long Pow(long @base, long exponent)
{
    var result = @base;

    for (var i = 1L; i < exponent; i++)
    {
        result *= @base;
    }

    return result;
}

void AddInvalidIds()
{
    var start = ToLong(startString);
    var end = ToLong(endString);

    Console.WriteLine($"{start}-{end}");

    var halfOrderMax = endString.Count / 2;

    for (var halfOrder = startString.Count / 2; halfOrder <= halfOrderMax; halfOrder++)
    {
        var half = halfOrder == 1 ? 1 : Pow(10, halfOrder - 1);
        var halfOffset = half * 10;
        var halfMax = halfOffset - 1;
        
        for (; half <= halfMax; half++)
        {
            var id = (half * halfOffset) + half;

            if (id < start)
            {
                continue;
            }

            if (id > end)
            {
                break;
            }

            sum += id;

            Console.WriteLine(id);
        }
    }
}

Console.WriteLine(sum);

static string GetInputFilePath(string inputFileName, [System.Runtime.CompilerServices.CallerFilePath] string? sourceCodePath = null)
{
    var sourceCodeDirectory = Path.GetDirectoryName(sourceCodePath);
    return Path.Combine(sourceCodeDirectory!, "input", inputFileName);
}