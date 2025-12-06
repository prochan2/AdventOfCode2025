var file = File.OpenRead(GetInputFilePath(
    //"sinput.txt"
    "input.txt"
));

var numbers = new List<long>();

var isFirstLine = true;
var numberIndex = 0;
int c;
var total = 0L;
Func<long, long>? operation = null;
long aggreagate = long.MinValue;

while ((c = file.ReadByte()) >= 0)
{
    if (c == ' ')
    {
        if (isFirstLine)
        {
            numbers.Add(-1L);
        }

        if (operation != null)
        {
            if (numbers[numberIndex] == -1L)
            {
                Console.WriteLine(aggreagate);
                total += aggreagate;
            }
            else
            {
                aggreagate = operation(numbers[numberIndex]);
            }
        }

        numberIndex++;

        continue;
    }

    if (char.IsDigit((char)c))
    {
        if (isFirstLine)
        {
            numbers.Add(c - '0');
        }
        else if (numbers[numberIndex] == -1L)
        {
            numbers[numberIndex] = c - '0';
        }
        else
        {
            numbers[numberIndex] = numbers[numberIndex] * 10 + (c - '0');
        }

        numberIndex++;
        continue;
    }

    if (c == '+')
    {
        aggreagate = numbers[numberIndex];
        operation = n => aggreagate + n;
        numberIndex++;
        continue;
    }

    if (c == '*')
    {
        aggreagate = numbers[numberIndex];
        operation = n => aggreagate * n;
        numberIndex++;
        continue;
    }

    if (c == '\n')
    {
        isFirstLine = false;
        numberIndex = 0;
        continue;
    }
}

Console.WriteLine(aggreagate);
total += aggreagate;

Console.WriteLine();
Console.WriteLine(total);

static string GetInputFilePath(string inputFileName, [System.Runtime.CompilerServices.CallerFilePath] string? sourceCodePath = null)
{
    var sourceCodeDirectory = Path.GetDirectoryName(sourceCodePath);
    return Path.Combine(sourceCodeDirectory!, "input", inputFileName);
}