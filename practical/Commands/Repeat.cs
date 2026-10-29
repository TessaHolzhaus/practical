using Practical.ProgramVisitors;
using Practical.Writing;

namespace Practical.Commands;

public class Repeat : ICommand, ICommandParser<Repeat>
{
    public List<ICommand> Commands { get; set; }
    public int Times { get; set; }

    public Repeat(int times) : this(times, []) {}
    public Repeat(int times, List<ICommand> commands)
    {
        Times = times;
        Commands = commands;
    }
    
    const string Prefix = "Repeat";
    const string Suffix = "times";

    
    public static Repeat Parse(string[] arguments, ILogger? logger = null)
    {
        if (arguments is not [{ } arg1, "times"] || !int.TryParse(arg1, out int times) || times < 0)
        {
            throw new ArgumentException("Repeat not valid");
        }

        return new Repeat(times);
    }

    public void Accept(ProgramVisitor visitor)
        => visitor.VisitRepeat(this);

    public void Execute(Player p)
    {
        if (!Commands.Any()) return;
        for (int i = 0; i < Times; i++)
        {
            foreach (ICommand command in Commands)
            {
                command.Execute(p);
            }
        }
    }
    
    public void WriteHeaderTo(TextWriter writer)
    {
        writer.Write(Prefix);
        writer.WriteSpace();
        writer.Write(Times);
        writer.WriteSpace();
        writer.Write(Suffix);
    }
}