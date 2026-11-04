using System.Diagnostics;
using System.Numerics;

namespace Practical;
public enum Field
{
    Empty,
    Wall,
    Finish
}

public class World
{
    public Player Player = new();
    public Field[,] Grid =
    {
        {Field.Empty, Field.Empty, Field.Wall, Field.Wall, Field.Wall},
        {Field.Wall, Field.Empty, Field.Empty, Field.Wall, Field.Wall},
        {Field.Wall, Field.Wall, Field.Empty, Field.Wall, Field.Wall},
        {Field.Wall, Field.Wall, Field.Empty, Field.Wall, Field.Wall},
        {Field.Wall, Field.Wall, Field.Empty, Field.Empty, Field.Finish},
    };

    public bool IsWithinBounds(Vector2 position)
    {
       return position.X >= 0 &&
              position.X < Grid.GetLength(1) && 
              position.Y <= 0 &&
              position.Y > -Grid.GetLength(0);
    }

    public bool IsWalkable(Vector2 position)
    {
        if (!IsWithinBounds(position)) return false;
        return Grid[Math.Abs((int)position.Y), (int)position.X] != Field.Wall;
    }

    public bool CanMoveTo(Vector2 position)
    {
        return IsWalkable(position);
    }
}