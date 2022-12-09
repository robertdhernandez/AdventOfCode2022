static IEnumerable<(Direction Direction, int Amount)> ReadInput(TextReader reader)
{
    string? line;
    while (!string.IsNullOrEmpty(line = reader.ReadLine()))
    {
        yield return ((Direction)line[0], int.Parse(line[2..]));
    }
}

int result = ReadInput(Console.In)
    .Scan(new MoveRecord(), (record, move) => record.Move(move.Direction, move.Amount))
    .SelectMany(rope => rope.TailMovement)
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

    public Vector2 Move(Direction dir, int amount) => this + (amount * FromDirection(dir));

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

    public static Vector2 operator *(int amount, Vector2 vec) => vec with
    {
        X = amount * vec.X,
        Y = amount * vec.Y,
    };
}

readonly struct Rope
{
    public Vector2 Head { get; private init; }
    public Vector2 Tail { get; private init; }

    public Rope Move(Direction dir, int amount) => Scan(dir, amount).Last();

    public IEnumerable<Rope> Scan(Direction dir, int amount)
    {
        Vector2 head = Head.Move(dir, amount);
        Vector2 tail, next = Tail;

        do
        {
            tail = next;
            yield return this with { Head = head, Tail = tail };
            next = tail.MoveTowards(head);
        } while (next != head);
    }
}

record class MoveRecord
{
    public Rope Rope { get; private init; }
    public IEnumerable<Vector2> TailMovement { get; private init; } = Enumerable.Empty<Vector2>();

    public MoveRecord Move(Direction dir, int amount) => this with
    {
        Rope = Rope.Move(dir, amount),
        TailMovement = Rope.Scan(dir, amount).Select(rope => rope.Tail),
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
