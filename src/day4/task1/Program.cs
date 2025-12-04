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
    
    rows[rowIndex][columnIndex].AdjacentRollsCount += additionalRollsCount;
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

rows.ForEach(row =>
{
    row.ForEach(position => Console.Write(position.IsRoll ? '@' : '.'));
    Console.WriteLine();
});

Console.WriteLine();

rows.ForEach(row =>
{
    row.ForEach(position => Console.Write(position.AdjacentRollsCount));
    Console.WriteLine();
});

Console.WriteLine();

var accessiblePositionsCount = rows.Aggregate(0L,
    (count, row) => count += row.Aggregate(0L,
    (rowCount, position) => rowCount += position.IsRoll && position.AdjacentRollsCount < 4 ? 1 : 0));

Console.WriteLine(accessiblePositionsCount);

static string GetInputFilePath(string inputFileName, [System.Runtime.CompilerServices.CallerFilePath] string? sourceCodePath = null)
{
    var sourceCodeDirectory = Path.GetDirectoryName(sourceCodePath);
    return Path.Combine(sourceCodeDirectory!, "input", inputFileName);
}

internal class Position
{
    public bool IsRoll { get; init; }

    public long AdjacentRollsCount { get; set; }
}