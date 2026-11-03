using System.Diagnostics;
using System.Numerics;
using Practical.ProgramVisitors;
using Practical.Writing;

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
        if (position.X < 0 || position.Y > 0 || position.X >= w.Grid.GetLength(1) ||
            Math.Abs(position.Y) >= w.Grid.GetLength(0))
            throw new Exception($"Player out of grid range: {(position.X, position.Y)}");
        return w.Grid[(int)position.Y, (int)position.X] != Field.Wall;
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
        return !(position.X < 0 || position.Y > 0 || position.X >= w.Grid.GetLength(1) || Math.Abs(position.Y) >= w.Grid.GetLength(0)); //Right way round?
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
        if (!Commands.Any()) return;
        foreach (ICommand command in Commands)
        {
            if (Condition.Check(w)) command.Execute(w); //TODO: Switch around commands
            else break;
        }
        if (Condition.Check(w)) Execute(w);
    }

    public void WriteHeaderTo(TextWriter writer)
    {
        writer.Write(Prefix);
        writer.WriteSpace();
        writer.Write(Condition);
    }
}