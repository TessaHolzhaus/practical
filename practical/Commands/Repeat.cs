using Practical.ProgramVisitors;

namespace Practical.Commands;

public class Repeat : ICommand
{
    public List<ICommand> Commands;
    // public IReadOnlyCollection<ICommand> Commands => _commands.AsReadOnly();

    private int Times { get; set; }

    public Repeat(List<ICommand> commands,int times)
    {
        Commands.AddRange(commands);
        Times = times;
    }

    public void Accept(ProgramVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public void Execute(Player p)
    {
        if (Commands.Count == 0) return;
        Console.Write($" Repeat {Times} times ");
        for (int i = 1; i < Times; i++)
        {
            foreach (ICommand command in Commands)
            {
                command.Execute(p);
            }
        }
    }
}