using System.Numerics;

namespace Practical.Commands;

public class Move : ICommand
{
    public int Distance { get; set; }

    public Move(int distance)
    {
        Distance = distance;
    }

    public void Execute(Player p)
    {
        switch(p.direction)
        {
           case Direction.North:
               p.Position += new Vector2(0, Distance); break;
           case Direction.East:
               p.Position += new Vector2(Distance, 0); break;
           case Direction.South:
               p.Position -= new Vector2(0, -Distance); break;
           case Direction.West:
               p.Position -= new Vector2(-Distance, 0); break;
        }
        Console.Write($" Move {Distance} ");
    }
}