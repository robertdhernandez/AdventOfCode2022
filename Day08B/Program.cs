using static System.Convert;

static Dictionary<Vector2, int> ReadInput(TextReader reader)
{
    Dictionary<Vector2, int> result = new();
    string? line;

    for (int y = 0; !string.IsNullOrEmpty(line = reader.ReadLine()); ++y)
    {
        for (int x = 0; x < line.Length; ++x)
        {
            if (int.TryParse(line.AsSpan(x, 1), out int value))
            {
                result[new Vector2(x, y)] = value;
            }
        }
    }

    return result;
}

Dictionary<Vector2, int> forest = ReadInput(Console.In);

int GetViewingDistance(Vector2 pos, int value, Direction dir)
{
    bool inBounds;
    int count = 1;
    while ((inBounds = forest.TryGetValue(pos = pos.GetNeighbor(dir), out int other)) && value > other)
    {
        ++count;
    }
    return Math.Max(1, count - ToInt32(!inBounds));
}

int GetScenicScore(KeyValuePair<Vector2, int> pair)
{
    return 1
        * GetViewingDistance(pair.Key, pair.Value, Direction.North)
        * GetViewingDistance(pair.Key, pair.Value, Direction.South)
        * GetViewingDistance(pair.Key, pair.Value, Direction.West)
        * GetViewingDistance(pair.Key, pair.Value, Direction.East);
}

Console.WriteLine(forest.Max(GetScenicScore));

[Flags]
enum Direction
{
    North = 1 << 0,
    South = 1 << 1,
    West = 1 << 2,
    East = 1 << 3,
}

record struct Vector2(int X, int Y)
{
    public Vector2 GetNeighbor(Direction dir) => this with
    {
        X = X + ToInt32(dir.HasFlag(Direction.East)) - ToInt32(dir.HasFlag(Direction.West)),
        Y = Y + ToInt32(dir.HasFlag(Direction.South)) - ToInt32(dir.HasFlag(Direction.North)),
    };
}
