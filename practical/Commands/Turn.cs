using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Practical.ProgramVisitors;
using Practical.Writing;

namespace Practical.Commands;

public enum RotationDirection
{
    Left,
    Right
}

public class Turn : ICommand, IParsableCommand
{
    public RotationDirection Rotation { get; set; }
    public ILogger? Logger { get; set; }
    
    
    public Turn(RotationDirection rotation)
    {
        Rotation = rotation;
    }
    
    private const string Prefix = "Turn";
    private const char Space = ' ';
    private const string Right = "right";
    private const string Left = "left";

    private static bool TryParseRotation(ReadOnlySpan<char> chars, out RotationDirection rotation)
    {
        switch (chars)
        {
            case Right:
                rotation = RotationDirection.Right;
                return true;
            case Left:
                rotation = RotationDirection.Left;
                return true;
            default:
                rotation = default;
                return false;
        }
    }
    
    public static bool TryParse(ReadOnlySpan<char> line, [MaybeNullWhen(false)] out ICommandBuilder builder)
    {
        if (
            line.StartsWith(Prefix) &&
            line[Prefix.Length..] is [Space, ..var maybeRotation] &&
            TryParseRotation(maybeRotation, out var rotation)
        ) {
            builder = new ScopelessCommandBuilder(logger => new Turn(rotation) { Logger = logger });
            return true;
        }

        builder = null;
        return false;
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
        writer.Write(Prefix);
        writer.Write(Space);
        writer.Write(Rotation switch {
            RotationDirection.Left => Left,
            RotationDirection.Right => Right,
            _ => throw new UnreachableException()
        });
    }
}