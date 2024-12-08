using Common;

namespace aoc2024.Days;

public class Day08 : IDay
{
    private char[,] _grid = new char[1, 1];
    private Dictionary<char, List<Point>> _antennas = [];
    public Tuple<string, string> Solve()
    {
        var lines = File.ReadAllLines(Path.Combine("Inputs", "Day08.txt"));
        _grid = new char[lines[0].Length, lines.Length];

        var j = 0;
        foreach (var line in lines)
        {
            var i = 0;
            foreach (var ch in line)
            {
                _grid[i, j] = ch;
                if (ch != '.')
                {
                    if (_antennas.TryGetValue(ch, out var list)) list.Add(new Point(i, j));
                    else _antennas[ch] = [new Point(i, j)];
                }
                i++;
            }

            j++;
        }

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
                foreach (var pointB in kvp.Value)
                {
                    if (pointA == pointB) continue;

                    if (includeMultiples) antinodes.Add(pointA);
                    var aToBVector = pointB - pointA;
                    var multiple = 1;
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
        return p.X < 0 || p.X >= _grid.GetLength(0)
            || p.Y < 0 || p.Y >= _grid.GetLength(1);
    }
}

internal record struct Point(int X, int Y)
{
    public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
    public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);
    public static Point operator *(int a, Point b) => new(a * b.X, a * b.Y);
}