using System.Diagnostics;
using System.Numerics;
using Practical.ProgramVisitors;
using Practical.Writing;

namespace Practical.Commands;

public class Move : ICommand, ICommandParser<Move>
{
    public int Distance { get; set; }
    public ILogger? Logger { get; set; }

    public Move(int distance)
    {
        Distance = distance;
    }
    
    private const string Prefix = "Move";

    public static Move Parse(string[] arguments, ILogger? logger = null)
    {
        if (arguments is not [{ } arg1] || !int.TryParse(arg1, out int distance))
            throw new ArgumentException("Move not valid");
        return new Move(distance) { Logger = logger };
    }

    public void Accept(ProgramVisitor visitor) 
        => visitor.VisitMove(this);

    public void Execute(World w)
    {
        Logger?.LogExecution(WriteHeaderTo);
        Coord nextPosition = w.Player.Position;
        for (int i = 0; i < Distance; i++)
        {
            nextPosition += w.Player.Direction.ToCoordDelta();
            if (!w.CanMoveTo(nextPosition)) // TODO: this only checks the final position. Shouldn't it move in steps??
                throw new Exception($"Cannot move to: {(nextPosition.X, nextPosition.Y)}");
        }
        w.Player.Position = nextPosition;
    }
    
    public void WriteHeaderTo(TextWriter writer)
    {
        writer.Write(Prefix);
        writer.WriteSpace();
        writer.Write(Distance);
    }
}