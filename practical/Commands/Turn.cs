using Practical.ProgramVisitors;

namespace Practical.Commands;

public enum RotationDirection
{
    left,
    right
}

public class Turn : ICommand
{
    public RotationDirection Rotation { get; set; }
    public Turn(RotationDirection rotation)
    {
        Rotation = rotation;
    }

    public void Accept(ProgramVisitor visitor)
    {
        visitor.Visit(this);
    }

    public void Execute(Player p)
    {
        if (p.direction is Direction.North && Rotation is RotationDirection.left) p.direction = Direction.West;
        if (p.direction is Direction.West && Rotation is RotationDirection.right) p.direction = Direction.North;

        var result = p.direction switch
        {
            Direction.North => Direction.East,
            Direction.East when Rotation is RotationDirection.left => Direction.North,
            Direction.East => Direction.South,
            Direction.South when Rotation is RotationDirection.left => Direction.East,
            Direction.South => Direction.West,
            Direction.West => Direction.South,
            _ => throw new NotImplementedException()
        };
        p.direction = result;
        Console.Write($" Turn {Rotation} ");
    }
}