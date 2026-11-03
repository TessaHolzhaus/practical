using System.Diagnostics;
using System.Numerics;
using Practical.ProgramVisitors;

namespace Practical.Commands;
public interface ICondition
{
    public bool Check(World w); //TODO: Implement grid
}

public class WallAhead : ICondition
{
    public bool Check(World w)
    {
        Vector2 position = w.Player.Position + w.Player.Direction switch
        {
            Direction.North => Vector2.UnitY,
            Direction.East => Vector2.UnitX,
            Direction.South => -Vector2.UnitY,
            Direction.West => -Vector2.UnitX,
            _ => throw new UnreachableException()
        };
        return w.Grid[(int)position.X, (int)position.Y] != Field.Wall;
    }
}

public class GridEdge : ICondition
{
    public bool Check(World w)
    {
        Vector2 position = w.Player.Position + w.Player.Direction switch
        {
            Direction.North => Vector2.UnitY,
            Direction.East => Vector2.UnitX,
            Direction.South => -Vector2.UnitY,
            Direction.West => -Vector2.UnitX,
            _ => throw new UnreachableException()
        };
        return position.X >= w.Grid.GetLength(1) || position.Y >= w.Grid.GetLength(0); //Right way round?
    }
}

public class RepeatUntil : ICommand, ICommandParser<RepeatUntil>
{
    private List<ICommand> _commands;
    public IReadOnlyCollection<ICommand> Commands;
    public ICondition Condition { get; set; }

    public RepeatUntil(ICondition c) : this(c, []){}
    public RepeatUntil(ICondition c, List<ICommand> commands)
    {
        Condition = c;
        _commands = commands;
    }
    
    public static RepeatUntil Parse(string[] arguments, ILogger? logger = null)
    {
        if (arguments is not [{ } arg1])
            throw new ArgumentException("RepeatUntil not valid");

        ICondition condition = arg1 switch
        {
            "WallAhead" => new WallAhead(),
            "GridEdge" => new GridEdge(),
            _ => throw new ArgumentException("Condition not valid")
        };
        return new RepeatUntil(condition);
    }
    
    public void Accept(ProgramVisitor visitor)
        => visitor.VisitRepeatUntil(this);

    public void Execute(World w)
    {
        if (!_commands.Any()) return;
        foreach (ICommand command in Commands)
        {
            if (Condition.Check(w)) command.Execute(w);
            else break;
        }
        if (Condition.Check(w)) Execute(w);
    }
}