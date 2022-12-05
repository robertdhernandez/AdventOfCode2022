static Shape ParseHand(char c) => c switch
{
    'A' => Shape.Rock,
    'B' => Shape.Paper,
    'C' => Shape.Scissors,
    _ => throw new FormatException(),
};

static Result ParseResult(char c) => c switch
{
    'X' => Result.Lose,
    'Y' => Result.Draw,
    'Z' => Result.Win,
    _ => throw new FormatException(),
};

static Result Flip(Result result) => result switch
{
    Result.Win => Result.Lose,
    Result.Lose => Result.Win,
    _ => result,
};

static Shape GetDesiredShape(Shape hand, Result result) => (hand, result) switch
{
    (Shape.Rock, Result.Win) => Shape.Scissors,
    (Shape.Rock, Result.Lose) => Shape.Paper,
    (Shape.Paper, Result.Win) => Shape.Rock,
    (Shape.Paper, Result.Lose) => Shape.Scissors,
    (Shape.Scissors, Result.Win) => Shape.Paper,
    (Shape.Scissors, Result.Lose) => Shape.Rock,
    _ => hand,
};

static Result GetRoundResult(Shape a, Shape b) => (a, b) switch
{
    (Shape.Rock, Shape.Scissors) => Result.Win,
    (Shape.Rock, Shape.Paper) => Result.Lose,
    (Shape.Paper, Shape.Rock) => Result.Win,
    (Shape.Paper, Shape.Scissors) => Result.Lose,
    (Shape.Scissors, Shape.Paper) => Result.Win,
    (Shape.Scissors, Shape.Rock) => Result.Lose,
    _ => Result.Draw,
};

static int GetRoundScore(Shape a, Shape b) => (int)a + (int)GetRoundResult(a, b);

static IEnumerable<(Shape a, Shape b)> ReadInput(TextReader input)
{
    string? line;
    while (!string.IsNullOrEmpty(line = input.ReadLine()))
    {
        Shape opponent = ParseHand(line[0]);
        Result result = ParseResult(line[2]);
        yield return (GetDesiredShape(opponent, Flip(result)), opponent);
    }
}

StreamReader input = new(Console.OpenStandardInput());
int result = ReadInput(input).Sum(g => GetRoundScore(g.a, g.b));

Console.Out.WriteLine(result);

enum Shape { Rock = 1, Paper = 2, Scissors = 3 }
enum Result { Win = 6, Draw = 3, Lose = 0 }
