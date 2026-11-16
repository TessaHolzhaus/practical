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

    public static World Create(string filepath = "BaseGrid.txt")
    {
        const string defaultPath = "../../../../practical/Grids";
        var fullPath = Path.Combine(defaultPath, filepath);

        
        return new(Grid.Parse(fullPath));
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

    public static Grid Parse(string gridpath)
    {
        var rows = new List<Field[]>();

        using (TextReader reader = new StreamReader(gridpath))
        {
            while (reader.ReadLine() is { } line)
            {
                var row = line.Select(c => c switch
                {
                    'o' => Field.Empty,
                    '+' => Field.Wall,
                    'x' => Field.Finish,
                    _ => throw new ArgumentException("Field type doesn't exist")
                }).ToArray();
                
                rows.Add(row);
            }
        }

        if (rows.Count == 0)
            throw new ArgumentException("Grid cannot be empty");
        
        int height = rows.Count;
        int width = rows[0].Length;

        if (rows.Any(row => row.Length != width))
            throw new ArgumentException("All rows must have the same length.");

        var fields = new Field [height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                fields[y, x] = rows[y][x];
            }
        }
        return new Grid(fields);
    }
}