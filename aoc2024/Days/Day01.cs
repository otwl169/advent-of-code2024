using Common;

namespace aoc2024.Days;

public class Day01 : Day
{
    private List<int> left = new List<int>();
    private List<int> right = new List<int>();
    
    public Tuple<string, string> Solve()
    {
        var lines  = File.ReadAllLines("Inputs\\Day01.txt");
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
        var total = 0;
        
        foreach (var id in right)
        {
            var current = counts.GetValueOrDefault(id, 0);
            counts[id] = current + 1;
        }

        foreach (var id in left)
        {
            total += id * counts.GetValueOrDefault(id, 0);
        }
        
        return total.ToString();
    }
}