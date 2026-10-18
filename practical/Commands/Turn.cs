using System.Diagnostics;
using Practical.ProgramVisitors;
using Practical.Writing;

namespace Practical.Commands;

public enum RotationDirection
{
    Left,
    Right
}

public class Turn : ICommandParser<Turn>
{
    public RotationDirection Rotation { get; set; }
    public Turn(RotationDirection rotation)
    {
        Rotation = rotation;
    }

    public static Turn Parse(string[] arguments) //TODO: Implement error checking
    {
        if (!Enum.TryParse(arguments[0], out RotationDirection rotation))
        {
            throw new Exception("Turn not valid");
        }
        return new Turn(rotation);
    }

    public void Accept(ProgramVisitor visitor)
        => visitor.VisitTurn(this);

    public void Execute(Player p)
    {
        p.Direction = p.Direction switch
        {
            Direction.North when Rotation is RotationDirection.Right => Direction.East,
            Direction.North when Rotation is RotationDirection.Left => Direction.West,
            Direction.East when Rotation is RotationDirection.Right => Direction.South,
            Direction.East when Rotation is RotationDirection.Left => Direction.North,
            Direction.South when Rotation is RotationDirection.Right => Direction.West,
            Direction.South when Rotation is RotationDirection.Left => Direction.East,
            Direction.West when Rotation is RotationDirection.Right => Direction.North,
            Direction.West when Rotation is RotationDirection.Left => Direction.South,
            _ => throw new UnreachableException()
        };
    }
    
    public void WriteHeaderTo(TextWriter writer)
    {
        writer.Write("Turn");
        writer.WriteSpace();
        writer.WriteLine(Rotation);
    }
}