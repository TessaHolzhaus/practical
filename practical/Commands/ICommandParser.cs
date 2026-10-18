namespace Practical.Commands;

public interface ICommandParser<TCommand> : ICommand
{
    static abstract TCommand Parse(string[] arguments);
}