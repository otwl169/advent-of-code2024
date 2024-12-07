using Common;

namespace aoc2024.Days;

public class Day02 : IDay
{
    private List<List<int>> _input = [];

    public Tuple<string, string> Solve()
    {
        var lines = File.ReadAllLines(Path.Combine("Inputs", "Day02.txt"));
        foreach (var val in lines.Select(l => l.Split(' ')))
        {
            var values = val.Select(v => int.Parse(v)).ToList();
            _input.Add(values);
        }

        return new Tuple<string, string>(Part1(), Part2());
    }

    private string Part1()
    {
        var total = 0;

        foreach (var input in _input)
        {
            var increasing = input[0] < input[1];
            var previous = increasing ? input[0] - 1 : input[0] + 1;
            var safe = true;
            foreach (var value in input)
            {
                if (increasing && value <= previous || !increasing && value >= previous) safe = false;
                if (int.Abs(value - previous) > 3) safe = false;
                previous = value;
            }

            if (safe) total++;
        }

        return total.ToString();
    }
    
    private string Part2()
    {
        var total = 0;
        foreach (var input in _input)
        {
            var numberOfStrictIncreases = 0;
            var numberOfStrictDecreases = 0;
            for (var i = 1; i < input.Count; i++)
            {
                if (input[i - 1] < input[i]) numberOfStrictIncreases++;
                if (input[i - 1] > input[i]) numberOfStrictDecreases++;
            }

            bool increasing;

            if (numberOfStrictIncreases >= input.Count - 2)
            {
                increasing = true;
            }
            else if (numberOfStrictDecreases >= input.Count - 2)
            {
                increasing = false;
            }
            else
            {
                continue; // neither increasing or decreasing after deleting one
            }

            var safe = true;
            var deletedIndex = -1;
            for (var i = 1; i < input.Count; i++)
            {
                if (i == deletedIndex || i-1 == deletedIndex) continue;

                if (BadPair(input[i-1], input[i], increasing))
                {
                    if (deletedIndex != -1) safe = false;
                    // we need to delete either i or i-1. greedily prefer to delete i, then delete i-1 if not possible
                    if (i + 1 >= input.Count || !BadPair(input[i - 1], input[i + 1], increasing))
                    {
                        deletedIndex = i;
                    }
                    else if (i - 2 < 0 || !BadPair(input[i - 2], input[i], increasing))
                    {
                        deletedIndex = i-1;
                    }
                    else
                    {
                        safe = false;
                    }
                }
            }

            if (safe) total++;
        }

        return total.ToString();
    }

    private static bool BadPair(int a1, int a2, bool increasing)
    {
        return (increasing && a1 >= a2 || !increasing && a1 <= a2 || int.Abs(a1 - a2) > 3);
    }
}