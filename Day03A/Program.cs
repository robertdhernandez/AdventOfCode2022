static (string, string) Split(string input) => (input.Substring(0, input.Length / 2), input.Substring(input.Length / 2));
static int GetPriority(char c) => char.ToLower(c) - 'a' + 1 + (26 * Convert.ToInt32(char.IsUpper(c)));

static IEnumerable<int> ReadInput(TextReader reader)
{
    string? line;
    while (!string.IsNullOrEmpty(line = reader.ReadLine()))
    {
        (string a, string b) = Split(line);
        yield return a.Intersect(b).Sum(GetPriority);
    }
}

int result = ReadInput(Console.In).Sum();
Console.WriteLine(result);
