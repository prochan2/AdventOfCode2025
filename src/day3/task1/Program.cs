var totalJoltage = 0L;

foreach (var bank in File.ReadLines(GetInputFilePath(
    //"sinput.txt"
    "input.txt"
)))
{
    var first = FindMax(bank, 0, bank.Length - 2);
    var second = FindMax(bank, first.Index + 1, bank.Length - 1);

    var bankJoltage = (first.Max * 10) + second.Max;

    Console.WriteLine(bankJoltage);

    totalJoltage += bankJoltage;
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