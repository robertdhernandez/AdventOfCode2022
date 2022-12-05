static int GetPriority(char c) => char.ToLower(c) - 'a' + 1 + (26 * Convert.ToInt32(char.IsUpper(c)));

static IEnumerable<char> ReadInput(TextReader reader)
{
    string? line;
    while (!string.IsNullOrEmpty(line = reader.ReadLine()))
    {
        yield return line
            .Intersect(reader.ReadLine()!)
            .Intersect(reader.ReadLine()!)
            .Single();
    }
}

int result = ReadInput(Console.In).Sum(GetPriority);
Console.WriteLine(result);
