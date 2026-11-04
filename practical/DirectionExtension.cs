using System.Diagnostics;
using System.Numerics;

namespace Practical;

public static class DirectionExtension
{
    public static Vector2 ToVector(this Direction direction)
    {
        return direction switch
        {
            Direction.North => Vector2.UnitY,
            Direction.East => Vector2.UnitX,
            Direction.South => -Vector2.UnitY,
            Direction.West => -Vector2.UnitX,
            _ => throw new UnreachableException()
        };
    }
}