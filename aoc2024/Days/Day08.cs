using Common;

namespace aoc2024.Days;

public class Day08 : IDay
{
    private readonly Dictionary<char, List<Point>> _antennas = [];
    private int _width = -1;
    private int _height = -1;

    public Tuple<string, string> Solve()
    {
        ParseInput();
        return new Tuple<string, string>(Part1(), Part2());
    }

    private string Part1()
    {
        return CountAntinodes(false).ToString();
    }
    
    private string Part2()
    {
        return CountAntinodes(true).ToString();
    }
    
    private int CountAntinodes(bool includeMultiples)
    {
        // For each antenna loop over all antennas of the same type and check for antinodes
        var antinodes = new HashSet<Point>();
        var maximumMultiple = includeMultiples ? int.MaxValue : 1;
        foreach (var kvp in _antennas)
        {
            foreach (var pointA in kvp.Value)
            {
                foreach (var pointB in kvp.Value.Where(pointB => pointA != pointB))
                {
                    var aToBVector = pointB - pointA;
                    var multiple = includeMultiples ? 0 : 1;
                    while (!OutOfBounds(pointA - multiple*aToBVector) && multiple <= maximumMultiple)
                    {
                        antinodes.Add(pointA - multiple*aToBVector);
                        multiple++;
                    }
                }
            }
        }

        return antinodes.Count(p => !OutOfBounds(p));
    }

    private bool OutOfBounds(Point p)
    {
        return p.X < 0 || p.X >= _width
            || p.Y < 0 || p.Y >= _height;
    }

    private void ParseInput()
    {
        var lines = File.ReadAllLines(Path.Combine("Inputs", "Day08.txt"));
        _width = lines[0].Length;
        _height = lines.Length;

        var j = 0;
        foreach (var line in lines)
        {
            var i = 0;
            foreach (var ch in line)
            {
                if (ch != '.')
                {
                    if (_antennas.TryGetValue(ch, out var list)) list.Add(new Point(i, j));
                    else _antennas[ch] = [new Point(i, j)];
                }
                i++;
            }

            j++;
        }
    }
}

internal record struct Point(int X, int Y)
{
    public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
    public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);
    public static Point operator *(int a, Point b) => new(a * b.X, a * b.Y);
}