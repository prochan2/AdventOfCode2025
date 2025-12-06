var file = File.OpenRead(GetInputFilePath(
    //"sinput.txt"
    "input.txt"
));

var sums = new List<long>();
var products = new List<long>();

var firstLine = true;
var skipSpace = false;
var i = 0;
int c;
var currentNumber = 0L;
var total = 0L;

while ((c = file.ReadByte()) >= 0)
{
    if (c == ' ' || c == '\n')
    {
        if (c == ' ' && skipSpace)
        {
            continue;
        }

        if (!skipSpace)
        {
            if (firstLine)
            {
                sums.Add(currentNumber);
                products.Add(currentNumber);
            }
            else
            {
                sums[i] += currentNumber;
                products[i] *= currentNumber;
            }

            Console.Write(currentNumber);
            Console.Write(' ');

            skipSpace = true;
            currentNumber = 0L;
        }

        if (c == '\n')
        {
            i = 0;
            firstLine = false;

            Console.WriteLine();
        }
        else
        {
            i++;
        }

        continue;
    }

    if (char.IsDigit((char)c))
    {
        currentNumber = currentNumber * 10 + (c - '0');
        skipSpace = false;
        continue;
    }

    if (c == '+')
    {
        total += sums[i];

        Console.Write(sums[i]);
        Console.Write(' ');

        i++;
        skipSpace = true;
        continue;
    }

    if (c == '*')
    {
        total += products[i];

        Console.Write(products[i]);
        Console.Write(' ');

        i++;
        skipSpace = true;
        continue;
    }
}

Console.WriteLine();
Console.WriteLine(total);

static string GetInputFilePath(string inputFileName, [System.Runtime.CompilerServices.CallerFilePath] string? sourceCodePath = null)
{
    var sourceCodeDirectory = Path.GetDirectoryName(sourceCodePath);
    return Path.Combine(sourceCodeDirectory!, "input", inputFileName);
}