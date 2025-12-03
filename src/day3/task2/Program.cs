var defaultColor = Console.ForegroundColor;

var totalJoltage = 0L;
const int cellsPerBank = 12;

foreach (var bank in File.ReadLines(GetInputFilePath(
//"sinput.txt"
"input.txt"
)))
{
    var previous = FindMax(bank, 0, bank.Length - cellsPerBank);
    WriteNext(bank, -1, previous.Index, previous.Max);

    var bankJoltage = previous.Max;

    for (int i = 1; i < cellsPerBank; i++)
    {
        var next = FindMax(bank, previous.Index + 1, bank.Length - cellsPerBank + i);
        WriteNext(bank, previous.Index, next.Index, next.Max);

        bankJoltage = (bankJoltage * 10) + next.Max;
        previous = next;
    }

    WriteNext(bank, previous.Index, bank.Length, -1);

    Console.WriteLine();

    totalJoltage += bankJoltage;
}

void WriteNext(string bank, int previousIndex, int nextIndex, long nextValue)
{
    Console.ForegroundColor = ConsoleColor.Red;

    for (int i = previousIndex + 1; i < nextIndex; i++)
    {
        Console.Write(bank[i]);
    }

    Console.ForegroundColor = ConsoleColor.Green;

    if (nextValue >= 0)
    {
        if (nextValue != bank[nextIndex] - '0')
        {
            throw new InvalidOperationException();
        }

        Console.Write(bank[nextIndex]);
    }

    Console.ForegroundColor = defaultColor;
}

Console.WriteLine(totalJoltage);

(long Max, int Index) FindMax(string s, int startIndex, int endIndex)
{
    var max = 0L;
    int maxIndex = 0;

    for (var i = startIndex; i <= endIndex; i++)
    {
        long value = s[i] - '0';

        if (value > max)
        {
            max = value;
            maxIndex = i;
        }
    }

    return (max, maxIndex);
}

static string GetInputFilePath(string inputFileName, [System.Runtime.CompilerServices.CallerFilePath] string? sourceCodePath = null)
{
    var sourceCodeDirectory = Path.GetDirectoryName(sourceCodePath);
    return Path.Combine(sourceCodeDirectory!, "input", inputFileName);
}