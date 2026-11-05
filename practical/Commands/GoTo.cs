using System.Numerics;
using Practical.ProgramVisitors;
using Practical.Writing;

namespace Practical.Commands;

public class GoTo : ICommand, ICommandParser<GoTo>
{
    public Vector2 Coords { get; set; }
    public ILogger? Logger { get; set; }
    
    public GoTo(Vector2 coords)
    {
        Coords = coords;
    }
    
    private const string Prefix = "GoTo";
    
    public static GoTo Parse(string[] arguments, ILogger? logger = null)
    {
        if (arguments is not [{ } arg1])
            throw new ArgumentException("GoTo not valid");
        return new GoTo(ParseVector(arg1)) { Logger = logger };
    }

    private static Vector2 ParseVector(string args)
    {
        string[] values = args.Trim('(', ')').Split(',');

        if (!int.TryParse(values[0], out int x) || !int.TryParse(values[1], out int y))
            throw new ArgumentException("GoTo not valid");
        return new Vector2(x, y);
    }
    public void Accept(ProgramVisitor visitor)
        => visitor.VisitGoTo(this);

    public void Execute(World w)
    {
        Logger?.LogExecution(WriteHeaderTo);
        if (!w.CanMoveTo(Coords))
            throw new Exception($"Cannot GoTo: {(Coords.X, Coords.Y)}");
        w.Player.Position = Coords;
    }

    private void WriteCoord(TextWriter writer) => writer.Write($"({Coords.X},{Coords.Y})");
    public void WriteHeaderTo(TextWriter writer)
    {
        writer.Write(Prefix);
        writer.WriteSpace();
        WriteCoord(writer);
    }
}