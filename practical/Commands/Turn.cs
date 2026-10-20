using System.Diagnostics;
using Practical.ProgramVisitors;
using Practical.Writing;

namespace Practical.Commands;

public enum RotationDirection
{
    Left,
    Right
}

public class Turn : ICommand, ICommandParser<Turn>
{
    public RotationDirection Rotation { get; set; }
    public ILogger? Logger { get; set; }
    
    
    public Turn(RotationDirection rotation)
    {
        Rotation = rotation;
    }

    public static Turn Parse(string[] arguments, ILogger? logger = null) //TODO: Implement error checking
    {
        if (arguments is not [{ } arg1])
            throw new ArgumentException("Expected 1 argument");
        
        var rotation = arg1 switch
        {
            "right" => RotationDirection.Right,
            "left" => RotationDirection.Left,
            _ => throw new ArgumentException("Turn not valid")
        };
        return new Turn(rotation) { Logger = logger };
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