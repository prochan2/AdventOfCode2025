//var start = 0L;
//var end = 0L;

//foreach (var line in File.ReadLines(GetInputFilePath("output.txt")))
//{
//    if (line.Contains("-"))
//    {
//        var parts = line.Split('-');
//        start = long.Parse(parts[0]);
//        end = long.Parse(parts[1]);
//        continue;
//    }

//    var id = long.Parse(line);
//    if (id < start || id > end)
//    {
//        throw new InvalidOperationException();
//    }

//    if (!ContainsSubsequence(id.ToString()))
//    {
//        throw new InvalidOperationException();
//    }
//}

//return;

var file = File.OpenRead(GetInputFilePath(
    //"sinput.txt"
    "input.txt"
));

// Sanity check - can be removed to improve performance.
var invalidIDs = new HashSet<long>();

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

static bool ContainsSubsequence(string sequence)
{
    for (var subsequenceLength = 1; subsequenceLength < sequence.Length; subsequenceLength++)
    {
        if (sequence.Length % subsequenceLength != 0)
        {
            continue;
        }

        var subsequence = sequence.Substring(0, subsequenceLength);
        var isSubsequence = true;

        for (int i = subsequenceLength; i < sequence.Length; i += subsequenceLength)
        {
            if (string.Compare(sequence, i, subsequence, 0, subsequenceLength, StringComparison.Ordinal) != 0)
            {
                isSubsequence = false;
                break;
            }
        }

        if (isSubsequence)
        {
            return true;
        }
    }

    return false;
}

void AddInvalidIds()
{
    var start = ToLong(startString);
    var end = ToLong(endString);

    Console.WriteLine($"{start}-{end}");

    for (var sequenceOrder = 1L; sequenceOrder * 2 <= endString.Count; sequenceOrder++)
    {
        for (var targetLength = startString.Count; targetLength <= endString.Count; targetLength++)
        {
            for (var repetitionsCount = targetLength; repetitionsCount >= 2; repetitionsCount--)
            {
                Console.WriteLine($"O{sequenceOrder} / L{targetLength} / C{repetitionsCount}");

                if (sequenceOrder * repetitionsCount != targetLength)
                {
                    continue;
                }

                var sequence = sequenceOrder == 1 ? 1 : Pow(10, sequenceOrder - 1);
                var sequenceOffset = sequence * 10;
                var sequenceMax = sequenceOffset - 1;

                for (; sequence <= sequenceMax; sequence++)
                {
                    var id = 0L;

                    for (var repetitionIndex = 0; repetitionIndex < repetitionsCount; repetitionIndex++)
                    {
                        id = (id * sequenceOffset) + sequence;
                    }

                    if (id < start)
                    {
                        continue;
                    }

                    if (id > end)
                    {
                        break;
                    }

                    if (sequenceOrder > 1 && ContainsSubsequence(sequence.ToString()))
                    {
                        continue;
                    }

                    if (invalidIDs.Contains(id))
                    {
                        throw new InvalidOperationException();
                    }

                    sum += id;

                    Console.WriteLine(id);
                    invalidIDs.Add(id);
                }
            }
        }
    }
}

Console.WriteLine(sum);

static string GetInputFilePath(string inputFileName, [System.Runtime.CompilerServices.CallerFilePath] string? sourceCodePath = null)
{
    var sourceCodeDirectory = Path.GetDirectoryName(sourceCodePath);
    return Path.Combine(sourceCodeDirectory!, "input", inputFileName);
}