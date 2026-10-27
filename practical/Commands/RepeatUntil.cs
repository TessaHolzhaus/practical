using System.Diagnostics;
using System.Numerics;
using Practical.ProgramVisitors;

namespace Practical.Commands;
public interface ICondition
{
    public bool Check(Player p); //TODO: Implement grid
}

public class WallAhead : ICondition
{
    public bool Check(Player p)
    {
        Vector2 position = p.Position + p.Direction switch
        {
            Direction.North => Vector2.UnitY,
            Direction.East => Vector2.UnitX,
            Direction.South => -Vector2.UnitY,
            Direction.West => -Vector2.UnitX,
            _ => throw new UnreachableException()
        };
        return true;
    }
}

public class GridEdge : ICondition
{
    public bool Check(Player p)
    {
        throw new NotImplementedException();
    }
}
public class RepeatUntil : ICommand, ICommandParser<RepeatUntil>
{
    private List<ICommand> _commands;
    public IReadOnlyCollection<ICommand> Commands;
    internal List<ICommand> MutCommands => _commands;
    
    public ICondition Condition { get; set; }
    public ILogger? Logger { get; set; }

    public RepeatUntil(ICondition c, List<ICommand> commands)
    {
        Condition = c;
        _commands = commands;
    }
    
    public static RepeatUntil Parse(string[] arguments, ILogger? logger = null)
    {
        throw new NotImplementedException();
    }
    
    public void Accept(ProgramVisitor visitor)
        => visitor.VisitRepeatUntil(this);

    public void Execute(Player p)
    {
        if (!_commands.Any()) return;
        foreach (ICommand command in Commands)
        {
            if (Condition.Check(p)) command.Execute(p);
            else break;
        }
        if (Condition.Check(p)) Execute(p);
    }
}