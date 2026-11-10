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
    public Field[,] Grid;

    public World(Field[,] grid)
    {
        Player = new Player();
        Grid = grid;
    }

    public bool IsWithinBounds(Coord position)
    {
       return position.X >= 0 &&
              position.X < Grid.GetLength(1) && 
              position.Y <= 0 &&
              position.Y > -Grid.GetLength(0);
    }

    public bool IsWalkable(Coord position)
    {
        if (!IsWithinBounds(position)) return false;
        return Grid[Math.Abs((int)position.Y), (int)position.X] != Field.Wall;
    }

    public bool CanMoveTo(Coord position)
    {
        return IsWalkable(position);
    }
}