using System.Security.Principal;

namespace Practical;

public enum Field
{
    Empty,
    Wall,
    Finish
}

public class World
{
    public Player Player;
    public Grid Grid;

    public World(Grid grid)
    {
        Player = new Player();
        Grid = grid;
    }

    public bool IsWithinBounds(Coord position)
    {
       return position.X >= 0 &&
              position.X < Grid.Width && 
              position.Y <= 0 &&
              position.Y > -Grid.Height; // TODO make the coordinates work from 0..height instead of -height
    }

    public bool IsWalkable(Coord position)
    {
        if (!IsWithinBounds(position)) return false;
        return Grid[Math.Abs(position.Y), position.X] is not Field.Wall;
    }

    public bool CanMoveTo(Coord position)
    {
        return IsWalkable(position);
    }
}

public class Grid(Field[,] grid)
{
    public Field this[int x, int y] => grid[x, y];
    
    public int Width => grid.GetLength(1);
    public int Height => grid.GetLength(0);
}