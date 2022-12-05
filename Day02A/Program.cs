static Hand Parse(char c) => c switch
{
    'A' or 'X' => Hand.Rock,
    'B' or 'Y' => Hand.Paper,
    'C' or 'Z' => Hand.Scissors,
    _ => throw new FormatException(),
};

static Result GetRoundResult(Hand a, Hand b) => (a, b) switch
{
    (Hand.Rock, Hand.Scissors) => Result.Win,
    (Hand.Rock, Hand.Paper) => Result.Lose,
    (Hand.Paper, Hand.Rock) => Result.Win,
    (Hand.Paper, Hand.Scissors) => Result.Lose,
    (Hand.Scissors, Hand.Paper) => Result.Win,
    (Hand.Scissors, Hand.Rock) => Result.Lose,
    _ => Result.Draw,
};

static int GetRoundScore(Hand a, Hand b) => (int)a + (int)GetRoundResult(a, b);

static IEnumerable<(Hand a, Hand b)> ReadInput(TextReader input)
{
    string? line;
    while (!string.IsNullOrEmpty(line = input.ReadLine()))
    {
        yield return (Parse(line[2]), Parse(line[0]));
    }
}

StreamReader input = new(Console.OpenStandardInput());
int result = ReadInput(input).Sum(g => GetRoundScore(g.a, g.b));

Console.Out.WriteLine(result);

enum Hand { Rock = 1, Paper = 2, Scissors = 3 }
enum Result { Win = 6, Draw = 3, Lose = 0 }
