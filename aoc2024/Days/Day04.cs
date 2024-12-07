using Common;

namespace aoc2024.Days;

public class Day04 : IDay
{
    private char[,] _grid = new char[0, 0];

    public Tuple<string, string> Solve()
    {
        var lines = File.ReadAllLines(Path.Combine("Inputs", "Day04.txt"));

        _grid = new char[lines[0].Length, lines.Length];
        var j = 0;
        foreach (var line in lines)
        {
            var i = 0;
            foreach (var ch in line)
            {
                _grid[i,j] = ch;
                i++;
            }
            j++;
        }

        return new Tuple<string, string>(Part1(), Part2());
    }

    private string Part1()
    {
        var total = 0;

        for (var y = 0; y < _grid.GetLength(1); y++)
        {
            for (var x = 0; x < _grid.GetLength(0); x++)
            {
                total += NumberOfXmasStartingAt(x, y);
            }
        }

        return total.ToString();
    }
    
    private string Part2()
    {
        var total = 0;

        for (var y = 0; y < _grid.GetLength(1); y++)
        {
            for (var x = 0; x < _grid.GetLength(0); x++)
            {
                total += IsCrossMas(x, y);
            }
        }

        return total.ToString();
    }

    private int NumberOfXmasStartingAt(int x, int y)
    {
        if (_grid[x, y] != 'X') return 0;

        // either +-x OR +-y OR +-x +-y
        var directions = new int[] { -1, 0, 1 };
        var numberOfXmas = 0;
        foreach (var x_inc in directions)
        {
            foreach (var y_inc in directions)
            {
                // check it won't go out of index exception
                if (3 * x_inc + x < 0
                    || 3 * x_inc + x >= _grid.GetLength(0)
                    || 3 * y_inc + y < 0
                    || 3 * y_inc + y >= _grid.GetLength(1))
                {
                    continue;
                }

                if (_grid[x, y] == 'X'
                    && _grid[x + x_inc, y + y_inc] == 'M'
                    && _grid[x + 2 * x_inc, y + 2 * y_inc] == 'A'
                    && _grid[x + 3 * x_inc, y + 3 * y_inc] == 'S')
                {
                    numberOfXmas++;
                }
            }
        }
        
        return numberOfXmas;
    }

    private int IsCrossMas(int x, int y)
    {
        if (_grid[x, y] != 'A') return 0;
        if (x - 1 < 0 || x + 1 >= _grid.GetLength(0)) return 0;
        if (y - 1 < 0 || y + 1 >= _grid.GetLength(1)) return 0;
        
        // top left to bottom right diagonal
        var diag1 = _grid[x - 1, y - 1] == 'M' && _grid[x + 1, y + 1] == 'S'
                     || _grid[x - 1, y - 1] == 'S' && _grid[x + 1, y + 1] == 'M';
        
        var diag2 = _grid[x + 1, y - 1] == 'M' && _grid[x - 1, y + 1] == 'S'
                     || _grid[x + 1, y - 1] == 'S' && _grid[x - 1, y + 1] == 'M';

        return (diag1 && diag2) ? 1 : 0;
    }
}