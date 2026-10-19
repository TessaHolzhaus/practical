namespace Practical;
using System.Numerics;
public enum Direction{
    North,
    East,
    South,
    West
}
public class Player
{
    public Direction Direction;
    public Vector2 Position { get; set; }

    
    private List<string> ActionLog = [];
    public string LoggedActions => string.Join(", ", ActionLog) + ".";
    public void LogAction(string action)
        => ActionLog.Add(action);
    
    public Player()
    {
        Direction = Direction.East;
        Position = new Vector2(0, 0);
    }
}