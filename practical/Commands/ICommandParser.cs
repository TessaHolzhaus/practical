namespace Practical.Commands;

public interface ICommandParser<TCommand>
{
    static abstract TCommand Parse(string[] arguments, ILogger? logger = null);
}