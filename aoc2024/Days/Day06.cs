using System.Runtime.InteropServices;
using Common;

namespace aoc2024.Days;

public class Day06 : IDay
{
    private char[,] _grid = new char[1, 1];
    private Position _guardStartingPosition = new(0, 0);
    private Direction _guardStartingDirection = Direction.North;

    private HashSet<Position> reachedPositionsInOriginalPath = new();
    
    public Tuple<string, string> Solve()
    {
        var lines = File.ReadAllLines(Path.Combine("Inputs", "Day06.txt"));
        _grid = new char[lines[0].Length, lines.Length];

        var j = 0;
        foreach (var line in lines)
        {
            var i = 0;
            foreach (var c in line)
            {
                _grid[i, j] = c;
                switch (c)
                {
                    case '<':
                        _guardStartingDirection = Direction.West;
                        _guardStartingPosition = new Position(i, j);
                        break;
                    case '>':
                        _guardStartingDirection = Direction.East;
                        _guardStartingPosition = new Position(i, j);
                        break;
                    case '^':
                        _guardStartingDirection = Direction.North;
                        _guardStartingPosition = new Position(i, j);
                        break;
                    case 'v':
                        _guardStartingDirection = Direction.South;
                        _guardStartingPosition = new Position(i, j);
                        break;
                }
                i++;
            }
            j++;
        }

        return new Tuple<string, string>(Part1(), Part2());
    }

    private string Part1()
    {
        var reachedPositions = new HashSet<Position>();
        var outOfBounds = false;
        var position = _guardStartingPosition;
        var nextPosition = _guardStartingPosition;
        var direction = _guardStartingDirection;
        while (!outOfBounds)
        {
            reachedPositions.Add(position);
            nextPosition = GoForward(position, direction);
            if (IsOutOfBounds(nextPosition))
            {
                outOfBounds = true;
            }
            else if (_grid[nextPosition.X, nextPosition.Y] == '#')
            {
                direction = TurnRight(direction);
            }
            else
            {
                position = nextPosition;
            }
        }

        reachedPositionsInOriginalPath = [..reachedPositions];
        return reachedPositions.Count.ToString();
    }

    private string Part2()
    {
        var total = 0;
        // only consider positions on the original path to reduce the search a bit
        foreach (var obstacleLocation in reachedPositionsInOriginalPath)
        {
            if (obstacleLocation == _guardStartingPosition) continue;

            var path = new HashSet<State>();
            var state = new State(_guardStartingPosition, _guardStartingDirection);
            var outOfBounds = false;
            var inLoop = false;
            while (!outOfBounds && !path.Contains(state))
            {
                path.Add(state);
                var nextPosition = GoForward(state.Position, state.Direction);
                if (IsOutOfBounds(nextPosition))
                {
                    outOfBounds = true;
                }
                else if (_grid[nextPosition.X, nextPosition.Y] == '#' || nextPosition == obstacleLocation)
                {
                    state = state with { Direction = TurnRight(state.Direction) };
                }
                else
                {
                    state = state with { Position = nextPosition };
                }
                
                inLoop = path.Contains(state) && !outOfBounds;
            }

            if (inLoop) total++;
        }

        return total.ToString();
    }

    private Position GoForward(Position position, Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return position with { Y = position.Y - 1 };
            case Direction.East:
                return position with { X = position.X + 1 };
            case Direction.South:
                return position with { Y = position.Y + 1 };
            case Direction.West:
                return position with { X = position.X - 1 };
            default:
                return position;
        }
    }

    private Direction TurnRight(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return Direction.East;
            case Direction.East:
                return Direction.South;
            case Direction.South:
                return Direction.West;
            case Direction.West:
                return Direction.North;
            default: // One day the compiler will know this is not possible
                return Direction.North;
        }
    }

    private bool IsOutOfBounds(Position position)
    {
        return position.X < 0 || position.X >= _grid.GetLength(0) || position.Y < 0 || position.Y >= _grid.GetLength(1);
    }
}

internal enum Direction
{
    North,
    East,
    South,
    West,
}

internal readonly record struct Position(int X, int Y);

internal readonly record struct State(Position Position, Direction Direction);