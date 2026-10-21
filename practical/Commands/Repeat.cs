using Practical.ProgramVisitors;
using Practical.Writing;

namespace Practical.Commands;

public class Repeat : ICommand, ICommandParser<Repeat>
{
    private List<ICommand> _commands;
    public IReadOnlyCollection<ICommand> Commands => _commands.AsReadOnly();
    internal List<ICommand> MutCommands => _commands;
    public int Times { get; set; }

    public Repeat(int times) : this(times, []) {}
    public Repeat(int times, List<ICommand> commands)
    {
        Times = times;
        _commands = commands;
    }
    
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
        if (!_commands.Any()) return;
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
        writer.Write("Repeat");
        writer.WriteSpace();
        writer.Write(Times);
        writer.WriteSpace();
        writer.Write("times");
    }
}