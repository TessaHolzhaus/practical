namespace Practical;
using System.Numerics;
public enum Direction{
    North,
    East,
    South,
    West
}
public class Player
{
    public Direction Direction;
    public Vector2 Position { get; set; }

    public Player()
    {
        Direction = Direction.East;
        Position = new Vector2(0, 0);
    }
}