var file = File.OpenRead(GetInputFilePath(
    //"sinput.txt"
    "input.txt"
));

var rows = new List<List<Position>>();
List<Position>? row = null;

long GetRollsCount(int rowIndex, int columnIndex)
{
    if (rowIndex < 0 || rowIndex >= rows.Count || columnIndex < 0 || columnIndex >= rows[0].Count)
    {
        return 0;
    }

    return rows[rowIndex][columnIndex].IsRoll ? 1 : 0;
}

void AddAdjacentRollsCount(int rowIndex, int columnIndex, long additionalRollsCount = 1)
{
    if (rowIndex < 0 || rowIndex >= rows.Count || columnIndex < 0 || columnIndex >= rows[0].Count)
    {
        return;
    }

    if (rows[rowIndex][columnIndex].AdjacentRollsCount + additionalRollsCount < 0)
    {
        rows[rowIndex][columnIndex].AdjacentRollsCount = 0;
    }
    else
    {
        rows[rowIndex][columnIndex].AdjacentRollsCount += additionalRollsCount;
    }
}

int c;
int rowIndex = 0;
int columnIndex = -1;

while ((c = file.ReadByte()) >= 0)
{
    if (row == null)
    {
        row = rows.Count > 0 ? new(rows[0].Count) : new();
        rows.Add(row);
    }

    if (c == '\n')
    {
        row = null;
        rowIndex++;
        columnIndex = -1;
        continue;
    }

    columnIndex++;

    if (c != '.' && c != '@')
    {
        continue;
    }

    var isRoll = c == '@';
    var adjacentRollsCount = 0L;

    adjacentRollsCount += GetRollsCount(rowIndex - 1, columnIndex - 1);
    adjacentRollsCount += GetRollsCount(rowIndex - 1, columnIndex);
    adjacentRollsCount += GetRollsCount(rowIndex - 1, columnIndex + 1);
    adjacentRollsCount += GetRollsCount(rowIndex, columnIndex - 1);

    if (isRoll)
    {
        AddAdjacentRollsCount(rowIndex - 1, columnIndex - 1);
        AddAdjacentRollsCount(rowIndex - 1, columnIndex);
        AddAdjacentRollsCount(rowIndex - 1, columnIndex + 1);
        AddAdjacentRollsCount(rowIndex, columnIndex - 1);
    }

    row.Add(new() { IsRoll = isRoll, AdjacentRollsCount = adjacentRollsCount });
}

//void Write(object? o = null) => Console.Write(o ?? "");
void Write(object? _ = null) { }

//void WriteLine(object? o = null) => Console.WriteLine(o ?? "");
void WriteLine(object? _ = null) { }

var removedRollsCount = 0L;
bool anyRollsRemoved;

var originalColor = Console.ForegroundColor;

do
{
    anyRollsRemoved = false;

    for (rowIndex = 0; rowIndex < rows.Count; rowIndex++)
    {
        for (columnIndex = 0; columnIndex < rows[rowIndex].Count; columnIndex++)
        {
            var position = rows[rowIndex][columnIndex];

            if (!position.IsAccessible)
            {
                if (position.IsRoll)
                {
                    Write('@');
                }
                else
                {
                    Write('.');
                }

                continue;
            }

            position.IsRoll = false;
            AddAdjacentRollsCount(rowIndex - 1, columnIndex - 1, -1);
            AddAdjacentRollsCount(rowIndex - 1, columnIndex, -1);
            AddAdjacentRollsCount(rowIndex - 1, columnIndex + 1, -1);
            AddAdjacentRollsCount(rowIndex, columnIndex - 1, -1);
            AddAdjacentRollsCount(rowIndex, columnIndex + 1, -1);
            AddAdjacentRollsCount(rowIndex + 1, columnIndex - 1, -1);
            AddAdjacentRollsCount(rowIndex + 1, columnIndex, -1);
            AddAdjacentRollsCount(rowIndex + 1, columnIndex + 1, -1);

            removedRollsCount++;
            anyRollsRemoved = true;

            Console.ForegroundColor = ConsoleColor.Red;
            Write('@');
            Console.ForegroundColor = originalColor;
        }

        WriteLine();
    }

    WriteLine();
}
while (anyRollsRemoved);

Console.WriteLine(removedRollsCount);

static string GetInputFilePath(string inputFileName, [System.Runtime.CompilerServices.CallerFilePath] string? sourceCodePath = null)
{
    var sourceCodeDirectory = Path.GetDirectoryName(sourceCodePath);
    return Path.Combine(sourceCodeDirectory!, "input", inputFileName);
}

internal class Position
{
    public bool IsRoll { get; set; }

    public long AdjacentRollsCount { get; set; }

    public bool IsAccessible => IsRoll && AdjacentRollsCount < 4;
}