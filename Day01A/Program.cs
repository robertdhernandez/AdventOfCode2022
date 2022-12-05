static IEnumerable<int> Read(StreamReader input)
{
    while (!input.EndOfStream)
    {
        string? line;
        int value = 0;

        while (!string.IsNullOrEmpty(line = input.ReadLine()))
        {
            value += int.Parse(line);
        }

        yield return value;
    }
}

StreamReader input = new(Console.OpenStandardInput());
Console.WriteLine(Read(input).Max());
