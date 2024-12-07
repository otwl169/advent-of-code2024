using System.Text.RegularExpressions;
using Common;

namespace aoc2024.Days;

public partial class Day03 : IDay
{
    [GeneratedRegex("mul\\((\\d{1,3}),(\\d{1,3})\\)")]
    private static partial Regex MyRegex();
    private static readonly Regex R = MyRegex();

    [GeneratedRegex("mul\\((\\d{1,3}),(\\d{1,3})\\)|do\\(\\)|don\\'t\\(\\)")]
    private static partial Regex MyRegex2();
    private static readonly Regex R2 = MyRegex2();

    private readonly string _text = File.ReadAllText("Inputs\\Day03.txt");

    public Tuple<string, string> Solve()
    {
        return new Tuple<string, string>(Part1(), Part2());
    }

    private string Part1()
    {
        var total = 0;
        foreach (Match match in R.Matches(_text))
        {
            total += int.Parse(match.Groups[1].Value)  * int.Parse(match.Groups[2].Value);
        }

        return total.ToString();
    }
    
    private string Part2()
    {
        var total = 0;
        var on = true;
        foreach (Match match in R2.Matches(_text))
        {
            if (match.Value.StartsWith("do(")) on = true;
            else if (match.Value.StartsWith("don")) on = false;
            else if (on)
            {
                total += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
            }
        }

        return total.ToString();
    }
}