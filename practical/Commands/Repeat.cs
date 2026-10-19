using Practical.ProgramVisitors;
using Practical.Writing;

namespace Practical.Commands;

public class Repeat : ICommand
{
    private List<ICommand> _commands;
    public IReadOnlyCollection<ICommand> Commands => _commands.AsReadOnly();
    public int Times { get; set; }


    public Repeat(int times, List<ICommand> commands)
    {
        _commands = commands;
        Times = times;
    }

    public void Accept(ProgramVisitor visitor)
        => visitor.VisitRepeat(this);

    public void Execute(Player p)
    {
        if (!_commands.Any()) return;
        for (int i = 1; i < Times; i++)
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