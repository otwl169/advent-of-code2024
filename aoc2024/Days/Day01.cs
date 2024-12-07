using Common;

namespace aoc2024.Days;

public class Day01 : IDay
{
    private List<int> left = [];
    private List<int> right = [];
    
    public Tuple<string, string> Solve()
    {
        var lines  = File.ReadAllLines(Path.Combine("Inputs", "Day01.txt"));
        foreach (var line in lines)
        {
            // Use known length of ints to avoid allocation of strings
            left.Add(int.Parse(line.AsSpan(0, 5)));
            right.Add(int.Parse(line.AsSpan(8, 5)));
        }

        return new Tuple<string, string>(Part1(), Part2());
    }
    
    private string Part1()
    {
        left.Sort();
        right.Sort();
        
        var sum = left.Zip(right, (a, b) => int.Abs(a - b)).Sum();
        return sum.ToString();
    }

    private string Part2()
    {
        var counts = new Dictionary<int, int>();

        foreach (var id in right)
        {
            var current = counts.GetValueOrDefault(id, 0);
            counts[id] = current + 1;
        }

        var total = left.Sum(id => id * counts.GetValueOrDefault(id, 0));

        return total.ToString();
    }
}