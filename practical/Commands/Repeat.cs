namespace Practical.Commands;

public class Repeat
{
    private List<ICommand> _commands;
    public IReadOnlyCollection<ICommand> Commands => _commands.AsReadOnly();
}