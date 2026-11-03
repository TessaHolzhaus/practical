namespace Practical;
public enum Field
{
    Empty,
    Wall,
    Finish
}

public class World
{
    public Player Player = new Player();
    public Field[,] Grid =
    {
        {Field.Empty, Field.Empty, Field.Wall, Field.Wall, Field.Wall},
        {Field.Wall, Field.Empty, Field.Empty, Field.Wall, Field.Wall},
        {Field.Wall, Field.Wall, Field.Empty, Field.Wall, Field.Wall},
        {Field.Wall, Field.Wall, Field.Empty, Field.Wall, Field.Wall},
        {Field.Wall, Field.Wall, Field.Empty, Field.Empty, Field.Finish},
    };
}