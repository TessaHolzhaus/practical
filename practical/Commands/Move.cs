using System.Numerics;
using Practical.ProgramVisitors;
using Practical.Writing;

namespace Practical.Commands;

public class Move : ICommand
{
    public int Distance { get; set; }

    public Move(int distance)
    {
        Distance = distance;
    }

    public void Accept(ProgramVisitor visitor) 
        => visitor.VisitMove(this);

    public void Execute(Player p)
    {
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
        writer.Write("Move");
        writer.WriteSpace();
        writer.WriteLine(Distance);
    }
}