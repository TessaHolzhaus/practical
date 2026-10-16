namespace Practical.Commands;

public class Repeat : ICommand
{
    private List<ICommand> Commands;
    private int Times { get; set; }

    public Repeat(List<ICommand> commands,int times)
    {
        Commands.AddRange(commands);
        Times = times;
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