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
    
    const string Prefix = "Move";

    public static Move Parse(string[] arguments, ILogger? logger = null)
    {
        if (arguments is not [{ } arg1] || !int.TryParse(arg1, out int distance))
        {
            throw new ArgumentException("Move not valid");
        }
        return new Move(distance) { Logger = logger };
    }

    public void Accept(ProgramVisitor visitor) 
        => visitor.VisitMove(this);

    public void Execute(World w)
    {
        Logger?.LogExecution(WriteHeaderTo);
        switch(w.Player.Direction)
        {
           case Direction.North:
               w.Player.Position += new Vector2(0, Distance); break;
           case Direction.East:
               w.Player.Position += new Vector2(Distance, 0); break;
           case Direction.South:
               w.Player.Position -= new Vector2(0, Distance); break;
           case Direction.West:
               w.Player.Position -= new Vector2(Distance, 0); break;
        }
    }
    
    public void WriteHeaderTo(TextWriter writer)
    {
        writer.Write(Prefix);
        writer.WriteSpace();
        writer.Write(Distance);
    }
}