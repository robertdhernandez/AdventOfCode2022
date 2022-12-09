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

bool IsVisible(Vector2 pos, int value, Direction dir)
{
    while (forest.TryGetValue(pos = pos.GetNeighbor(dir), out int other))
    {
        if (value <= other)
        {
            return false;
        }
    }
    return true;
}

Direction GetVisibility(Vector2 point, int value)
{
    return default(Direction)
        | (Direction)((int)Direction.North * ToInt32(IsVisible(point, value, Direction.North)))
        | (Direction)((int)Direction.South * ToInt32(IsVisible(point, value, Direction.South)))
        | (Direction)((int)Direction.West * ToInt32(IsVisible(point, value, Direction.West)))
        | (Direction)((int)Direction.East * ToInt32(IsVisible(point, value, Direction.East)));
}

int result = forest.Count(p => GetVisibility(p.Key, p.Value) != 0);
Console.WriteLine(result);

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
