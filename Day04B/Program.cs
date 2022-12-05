using System.Text.RegularExpressions;

static IEnumerable<(Range x, Range y)> ReadInput(TextReader input)
{
    string? line;
    while (!string.IsNullOrEmpty(line = input.ReadLine()))
    {
        Match match = Regex.Match(line, @"(.*),(.*)");
        yield return (Range.Parse(match.Groups[1].Value), Range.Parse(match.Groups[2].Value));
    }
}

int result = ReadInput(Console.In).Count(p => p.x.HasOverlap(p.y));
Console.WriteLine(result);

record struct Range(int Start, int End)
{
    public static Range Parse(string input)
    {
        Match match = Regex.Match(input, @"(\d+)-(\d+)");
        return new Range
        {
            Start = int.Parse(match.Groups[1].Value),
            End = int.Parse(match.Groups[2].Value),
        };
    }

    public bool HasOverlap(Range other) => Start <= other.End && End >= other.Start;
}
