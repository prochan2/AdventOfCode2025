var junctionBoxes = new LinkedList<JunctionBox>();

await foreach (var line in File.ReadLinesAsync(GetInputFilePath(
    //"sinput.txt"
    "input.txt"
)))
{
    var parts = line.Split(',');
    
    var junction = new JunctionBox(
        double.Parse(parts[0]),
        double.Parse(parts[1]),
        double.Parse(parts[2])
    );

    junctionBoxes.AddLast(junction);
}

var circuits = new Dictionary<JunctionBox, Circuit>();

for (int i = 0; i < 10; i++)
{
    double minDistance = double.MaxValue;
    JunctionBox? closestJunktionBoxA = null;
    JunctionBox? closestJunktionBoxB = null;

    foreach (var junctionBoxA in junctionBoxes)
    {
        foreach (var junctionBoxB in junctionBoxes)
        {
            if (junctionBoxA == junctionBoxB)
            {
                continue;
            }

            if (junctionBoxA.Junktions.Any(j => j.BoxA == junctionBoxB || j.BoxB == junctionBoxB))
            {
                continue;
            }

            var distance = junctionBoxA.DistanceTo(junctionBoxB);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestJunktionBoxA = junctionBoxA;
                closestJunktionBoxB = junctionBoxB;
            }
        }
    }

    var junktion = Junktion.Join(closestJunktionBoxA!, closestJunktionBoxB!);

    var circuitA = circuits.GetValueOrDefault(junktion.BoxA);
    var circuitB = circuits.GetValueOrDefault(junktion.BoxB);

    if (circuitA == null && circuitB == null)
    {
        var circuit = new Circuit();
        circuit.Add(junktion.BoxA);
        circuit.Add(junktion.BoxB);
        circuits[junktion.BoxA] = circuit;
        circuits[junktion.BoxB] = circuit;
    }
    else if (circuitA == null && circuitB != null)
    {
        circuitB.Add(junktion.BoxA);
        circuits[junktion.BoxA] = circuitB;
    }
    else if (circuitA != null && circuitB == null)
    {
        circuitA.Add(junktion.BoxB);
        circuits[junktion.BoxB] = circuitA;
    }
    else if (circuitA != null && circuitB != null && circuitA != circuitB)
    {
        circuitA.Merge(circuitB);

        foreach (var junctionBox in circuitB.JunctionBoxes)
        {
            circuits[junctionBox] = circuitA;
        }
    }
}

var largestCircuits = new HashSet<Circuit>();

for (int i = 0; i < 3; i++)
{
    var largestCircuitSize = 0L;
    Circuit? largestCircuit = null;

    foreach (var circuit in circuits.Values)
    {
        if (largestCircuits.Contains(circuit))
        {
            continue;
        }

        if (circuit.Size > largestCircuitSize)
        {
            largestCircuitSize = circuit.Size;
            largestCircuit = circuit;
        }
    }

    largestCircuits.Add(largestCircuit ?? throw new InvalidOperationException());
}

Console.WriteLine(largestCircuits.Aggregate(1L, (acc, circuit) => acc * circuit.Size));

static string GetInputFilePath(string inputFileName, [System.Runtime.CompilerServices.CallerFilePath] string? sourceCodePath = null)
{
    var sourceCodeDirectory = Path.GetDirectoryName(sourceCodePath);
    return Path.Combine(sourceCodeDirectory!, "input", inputFileName);
}

class Circuit
{
    private readonly HashSet<JunctionBox> _junctionBoxes = new();

    public int Size => _junctionBoxes.Count;

    public IReadOnlySet<JunctionBox> JunctionBoxes => _junctionBoxes;

    public bool Contains(JunctionBox box) => _junctionBoxes.Contains(box);

    public void Add(JunctionBox box) => _junctionBoxes.Add(box);

    public void Remove(JunctionBox box) => _junctionBoxes.Remove(box);

    public void Merge(Circuit other)
    {
        foreach (var box in other._junctionBoxes)
        {
            _junctionBoxes.Add(box);
        }
    }
}

class Junktion
{
    public JunctionBox BoxA { get; }

    public JunctionBox BoxB { get; }

    public double Distance { get; }

    private Junktion(JunctionBox boxA, JunctionBox boxB, double distance)
    {
        BoxA = boxA;
        BoxB = boxB;
        Distance = distance;
    }

    public static Junktion Join(JunctionBox boxA, JunctionBox boxB)
    {
        var distance = boxA.DistanceTo(boxB);
        var junktion = new Junktion(boxA, boxB, distance);
        boxA.AddJunktion(junktion);
        boxB.AddJunktion(junktion);

        return junktion;
    }
}

class JunctionBox
{
    private readonly List<Junktion> _junktions = new();

    public double X { get; }

    public double Y { get; }

    public double Z { get; }

    public IReadOnlyList<Junktion> Junktions => _junktions;

    public JunctionBox(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public double DistanceTo(JunctionBox other)
    {
        var dx = X - other.X;
        var dy = Y - other.Y;
        var dz = Z - other.Z;
        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    public void AddJunktion(Junktion junktion)
    {
        _junktions.Add(junktion);
    }
}