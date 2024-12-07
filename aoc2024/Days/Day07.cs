using System.Text.RegularExpressions;
using Common;

namespace aoc2024.Days;

public partial class Day07 : IDay
{
    [GeneratedRegex(@"(\d+): (\d+(?: \d+)*)")]
    private static partial Regex EquationRegex();
    private Regex Regex = EquationRegex();

    private List<(long total, List<int> rhs)> _equations = [];

    public Tuple<string, string> Solve()
    {
        var text = File.ReadAllText(Path.Combine("Inputs", "Day07.txt"));

        foreach (Match match in Regex.Matches(text))
        {
            var total = long.Parse(match.Groups[1].Value);
            var rhs = match.Groups[2].Value.Split(' ').Select(int.Parse).ToList();
            _equations.Add((total,rhs));
        }

        return new Tuple<string, string>(Part1(), Part2());
    }

    private string Part1()
    {
        return _equations
            .Where(x => CanMakeTotal(x.total, x.rhs[0], x.rhs.Skip(1).ToArray()))
            .Sum(x => x.total)
            .ToString();
    }

    private string Part2()
    {
        return _equations
            .Where(x => CanMakeTotalWithConcat(x.total, x.rhs[0], x.rhs.Skip(1).ToArray()))
            .Sum(x => x.total)
            .ToString();
    }

    private bool CanMakeTotal(long total, long current, ReadOnlySpan<int> rhs)
    {
        if (current > total) return false;
        if (rhs.Length == 0) return total == current;

        var withMul = current * rhs[0];
        var withSum = current + rhs[0];
        
        return CanMakeTotal(total, withMul, rhs[1..]) || CanMakeTotal(total, withSum, rhs[1..]);
    }
    
    private bool CanMakeTotalWithConcat(long total, long current, ReadOnlySpan<int> rhs)
    {
        if (current > total) return false;
        if (rhs.Length == 0) return total == current;

        var withMul = current * rhs[0];
        var withSum = current + rhs[0];
        var withConcat = ConcatNumbers(current, rhs[0]);
        
        return CanMakeTotalWithConcat(total, withMul, rhs[1..])
               || CanMakeTotalWithConcat(total, withSum, rhs[1..])
               || CanMakeTotalWithConcat(total, withConcat, rhs[1..]);
    }

    private long ConcatNumbers(long a, long b)
    {
        var asString = a + b.ToString();
        return long.Parse(asString);
    }
}