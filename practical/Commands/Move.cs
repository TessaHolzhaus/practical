using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Practical.ProgramVisitors;
using Practical.Writing;

namespace Practical.Commands;

public class Move : ICommand, IParsableCommand
{
    public int Distance { get; set; }
    public ILogger? Logger { get; set; }

    public Move(int distance)
    {
        Distance = distance;
    }
    
    private const string Prefix = "Move";
    private const char Space = ' ';
    
    public static bool TryParse(ReadOnlySpan<char> line, [MaybeNullWhen(false)] out ICommandBuilder builder)
    {
        if (
            line.StartsWith(Prefix) &&
            line[Prefix.Length..] is [Space, ..var maybeNumber] &&
            int.TryParse(maybeNumber, out var distance)
        ) {
            builder = new ScopelessCommandBuilder(logger => new Move(distance) { Logger = logger });
            return true;
        }

        builder = null;
        return false;
    }

    public void Accept(ProgramVisitor visitor) 
        => visitor.VisitMove(this);

    public void Execute(Player p)
    {
        Logger?.LogExecution(WriteHeaderTo);
        switch(p.Direction)
        {
           case Direction.North:
               p.Position += new Vector2(0, Distance); break;
           case Direction.East:
               p.Position += new Vector2(Distance, 0); break;
           case Direction.South:
               p.Position -= new Vector2(0, Distance); break;
           case Direction.West:
               p.Position -= new Vector2(Distance, 0); break;
        }
    }
    
    public void WriteHeaderTo(TextWriter writer)
    {
        writer.Write(Prefix);
        writer.Write(Space);
        writer.Write(Distance);
    }
}