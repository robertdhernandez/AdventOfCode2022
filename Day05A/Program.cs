using System.Text.RegularExpressions;

static IEnumerable<string> ReadLines(TextReader reader)
{
    string? line;
    while (!string.IsNullOrEmpty(line = reader.ReadLine()))
    {
        yield return line;
    }
}

static Stack<char>[] ReadCargo(TextReader reader)
{
    string[] lines = ReadLines(reader)
        .TakeWhile(s => !string.IsNullOrEmpty(s))
        .ToArray();

    var stacks = new Stack<char>[(lines[0].Length + 1) / 4];

    for (int i = 0; i < stacks.Length; ++i)
    {
        stacks[i] = new();
    }

    for (int i = lines.Length - 2; i >= 0; --i)
    {
        for (int j = 0; j < stacks.Length; ++j)
        {
            char c = lines[i][1 + j * 4];

            if (char.IsLetter(c))
            {
                stacks[j].Push(c);
            }
        }
    }

    return stacks;
}

static Action<Stack<char>[]> ParseAction(string input)
{
    Match match = Regex.Match(input, @"move (\d+) from (\d+) to (\d+)");

    int amount = int.Parse(match.Groups[1].Value);
    int source = int.Parse(match.Groups[2].Value) - 1;
    int target = int.Parse(match.Groups[3].Value) - 1;

    return stack =>
    {
        for (int i = 0; i < amount && stack[source].TryPop(out char c); i++)
        {
            stack[target].Push(c);
        }
    };
}

Stack<char>[] stack = ReadCargo(Console.In);

foreach (var action in ReadLines(Console.In).Select(ParseAction))
{
    action(stack);
}

string result = string.Concat(stack.Select(s => s.Peek()));

Console.WriteLine(result);
