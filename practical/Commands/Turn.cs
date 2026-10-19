using System.Diagnostics;
using Practical.ProgramVisitors;
using Practical.Writing;

namespace Practical.Commands;

public enum RotationDirection
{
    Left,
    Right
}

public class Turn : ICommand
{
    public RotationDirection Rotation { get; set; }
    public ILogger? Logger { get; set; }
    
    
    public Turn(RotationDirection rotation)
    {
        Rotation = rotation;
    }

    public void Accept(ProgramVisitor visitor)
        => visitor.VisitTurn(this);

    public void Execute(Player p)
    {
        Logger?.LogExecution(WriteHeaderTo);
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
        writer.Write(Rotation.ToString().ToLowerInvariant());
    }
}