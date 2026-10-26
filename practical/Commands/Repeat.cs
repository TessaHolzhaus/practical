using System.Diagnostics.CodeAnalysis;
using Practical.ProgramVisitors;
using Practical.Writing;

namespace Practical.Commands;

public class Repeat : ICommand, IParsableCommand
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

    private const string Prefix = "Repeat";
    private const char Space = ' ';
    private const string Suffix = "times";
    
    public static bool TryParse(ReadOnlySpan<char> line, [MaybeNullWhen(false)] out ICommandBuilder builder)
    {
        if (!line.StartsWith(Prefix) ||
            line[Prefix.Length..] is not [Space, .. var maybeArgs])
        {
            builder = null;
            return false;
        }
        
        var separator = maybeArgs.IndexOf(Space);

        if (separator < 0)
        {
            builder = null;
            return false;
        }
            
        if(!int.TryParse(maybeArgs[..separator], out var times) || maybeArgs[separator..] is Suffix)
        {
            builder = null;
            return false;
        }

        builder = new Builder(times);
        return true;
    }

    private class Builder(int times) : ICommandBuilder
    {
        private readonly List<ICommand> _commands = [];

        public void UseLogger(ILogger? logger) {}
        public bool TryAddCommand(ICommand command)
        {
            _commands.Add(command);
            return  true;
        }

        public bool TryOpenNextScope(ReadOnlySpan<char> header) => false;
        public ICommand Build() => new Repeat(times, _commands);
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