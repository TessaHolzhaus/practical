using System.Diagnostics.CodeAnalysis;

namespace Practical.Commands;

public interface IParsableCommand
{
    static abstract bool TryParse(ReadOnlySpan<char> line, [MaybeNullWhen(false)] out ICommandBuilder builder);
}

public interface ICommandParser
{
    bool TryParse(ReadOnlySpan<char> line, [MaybeNullWhen(false)] out ICommandBuilder builder);
}

public struct CommandParser<T> : ICommandParser where T : IParsableCommand
{
    public bool TryParse(ReadOnlySpan<char> line, [MaybeNullWhen(false)] out ICommandBuilder builder) 
        => T.TryParse(line, out builder);
}

public interface ICommandBuilder
{
    void UseLogger(ILogger? logger);
    bool TryAddCommand(ICommand command);
    bool TryOpenNextScope(ReadOnlySpan<char> header);
    ICommand Build();
}

public class ScopelessCommandBuilder(Func<ILogger?, ICommand> factory) : ICommandBuilder
{
    protected ILogger? Logger;
    
    public void UseLogger(ILogger? logger) => Logger = logger;
    public bool TryAddCommand(ICommand command) => false;
    public bool TryOpenNextScope(ReadOnlySpan<char> header) => false;
    public ICommand Build() => factory(Logger);
}