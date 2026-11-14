using System.Numerics;
using Practical.ProgramVisitors;
using Practical.Writing;

namespace Practical.Commands;
public interface ICondition
{
    public bool Check(World w);
}

public class WallAhead : ICondition
{
    public bool Check(World w)
    {
        Coord nextPosition = w.Player.Position + w.Player.Direction.ToCoordDelta();
        return w.IsWalkable(nextPosition);
    }
}

public class GridEdge : ICondition
{
    public bool Check(World w)
    {
        Coord nextPosition = w.Player.Position + w.Player.Direction.ToCoordDelta();
        return w.IsWithinBounds(nextPosition);
    }
}

public class RepeatUntil : ICommand, ICommandParser<RepeatUntil>
{
    public List<ICommand> Commands { get; set; }
    public ICondition Condition { get; set; }

    public RepeatUntil(ICondition c) : this(c, []){}
    public RepeatUntil(ICondition c, List<ICommand> commands)
    {
        Condition = c;
        Commands = commands;
    }
    
    const string Prefix = "RepeatUntil";
    
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
        while (Commands.Any() && Condition.Check(w))
        {
            foreach (ICommand command in Commands)
            {
                if (!Condition.Check(w)) return;
                command.Execute(w);
            }
        }
    }

    public void WriteHeaderTo(TextWriter writer)
    {
        writer.Write(Prefix);
        writer.WriteSpace();
        writer.Write(Condition);
    }
}