using System.Diagnostics;
using System.Numerics;

namespace Practical;

public static class DirectionExtension
{
    public static Coord ToCoordDelta(this Direction direction)
    {
        return direction switch
        {
            Direction.North => new Coord(0, 1),
            Direction.East => new Coord(1, 0),
            Direction.South => new Coord(0, -1),
            Direction.West => new Coord(-1, 0),
            _ => throw new UnreachableException()
        };
    }
}