using Practical.ProgramVisitors;
using Practical.Writing;

namespace Practical.Commands;

public class Repeat : ICommandParser<Repeat>
{
    private List<ICommand> _commands;
    public IReadOnlyCollection<ICommand> Commands => _commands.AsReadOnly();
    public int Times { get; set; }


    public Repeat(int times) : this(times, []) {}
    public Repeat(int times, List<ICommand> commands)
    {
        Times = times;
        _commands = commands;
    }

    
    public static Repeat Parse(string[] arguments) //TODO: Implement error checking
    {
        if (!arguments.Any() || !int.TryParse(arguments[0], out int times) || times < 0)
        {
            throw new Exception("Repeat not valid");
        }
        
        return new Repeat(times);
    }

    public void AddCommand(ICommand command)
    {
        _commands.Add(command);
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
        writer.WriteLine("times");
    }
}