static IEnumerable<(Direction Direction, int Amount)> ReadInput(TextReader reader)
{
    string? line;
    while (!string.IsNullOrEmpty(line = reader.ReadLine()))
    {
        yield return ((Direction)line[0], int.Parse(line[2..]));
    }
}

int result = ReadInput(Console.In)
    .Scan(new MoveRecord(new Rope(10)), (record, move) => record.Move(move.Direction, move.Amount))
    .SelectMany(record => record.Movement.Select(rope => rope.Tail))
    .Distinct()
    .Count();

Console.WriteLine(result);

enum Direction
{
    Up = 'U',
    Down = 'D',
    Left = 'L',
    Right = 'R',
}

record struct Vector2(int X, int Y)
{
    public static readonly Vector2 Up = new(0, -1);
    public static readonly Vector2 Down = new(0, 1);
    public static readonly Vector2 Left = new(-1, 0);
    public static readonly Vector2 Right = new(1, 0);

    public Vector2 MoveTowards(Vector2 target) => this with
    {
        X = X + Math.Sign(target.X - X),
        Y = Y + Math.Sign(target.Y - Y),
    };

    public static Vector2 FromDirection(Direction dir) => dir switch
    {
        Direction.Up => Up,
        Direction.Down => Down,
        Direction.Left => Left,
        Direction.Right => Right,
        _ => default,
    };

    public static Vector2 operator +(Vector2 a, Vector2 b) => a with
    {
        X = a.X + b.X,
        Y = a.Y + b.Y,
    };
}

sealed class Rope
{
    public Rope(int knots)
    {
        Knots = new Vector2[knots];
    }

    private Rope(Vector2[] knots)
    {
        Knots = knots;
    }

    public Vector2 Head => Knots[0];
    public Vector2 Tail => Knots[^1];

    private Vector2[] Knots { get; }

    public IEnumerable<Rope> Scan(Direction dir, int amount)
    {
        var knots = (Vector2[])Knots.Clone();

        while (amount-- > 0)
        {
            knots[0] += Vector2.FromDirection(dir);

            for (int i = 1; i < knots.Length; i++)
            {
                Vector2 next = knots[i].MoveTowards(knots[i - 1]);

                if (next != knots[i - 1])
                {
                    knots[i] = next;
                }
            }

            yield return new Rope(knots);
        }
    }
}

record class MoveRecord
{
    public MoveRecord(Rope seed) => Current = seed;

    public Rope Current { get; private init; }
    public IEnumerable<Rope> Movement { get; private init; } = Enumerable.Empty<Rope>();

    public MoveRecord Move(Direction dir, int amount) => this with
    {
        Current = Current.Scan(dir, amount).Last(),
        Movement = Current.Scan(dir, amount),
    };
}

static class EnumerableExtensions
{
    public static IEnumerable<TAccumulate> Scan<T, TAccumulate>(this IEnumerable<T> values, TAccumulate seed, Func<TAccumulate, T, TAccumulate> func)
    {
        foreach (T value in values)
        {
            yield return seed = func(seed, value);
        }
    }
}
