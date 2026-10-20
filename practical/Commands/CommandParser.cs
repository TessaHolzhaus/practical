using Practical.Writing;

namespace Practical.Commands;

public class CommandParser(ILogger? logger = null)
{
    public ILogger? Logger { get; set; } = logger;
    public Indent Indent { get; set; } = new(' ', 4);

    private List<ICommand> _programScope = [];
    private Stack<ICollection<ICommand>> _scopesStack = [];
    private int _currentIndentation = 0;

    private void EnterScope(ICollection<ICommand> scope)
    {
        _scopesStack.Push(scope);
        _currentIndentation++;
    }

    private void ExitScope()
    {
        _scopesStack.Pop();
        _currentIndentation--;
    }

    private ICollection<ICommand> CurrentScope => _scopesStack.TryPeek(out var result) ? result : _programScope;

    private void Reset()
    {
        _scopesStack = [];
        _programScope = [];
    }
    
    public List<ICommand> CommandParse(TextReader reader)
    {
        Reset();
        while (reader.ReadLine() is { } line)
        {
            var commandAsChars = Indent.TrimFromStart(line.UnsafeMoveToSpan(), out var indentation);
            if (commandAsChars.IsEmpty)
                throw new InvalidOperationException($"After trimming the indentation {indentation} times no characters where left");

            if (char.IsWhiteSpace(commandAsChars[0]))
            {
                if (commandAsChars[1..].IsWhiteSpace()) continue;
                throw new InvalidOperationException($"After trimming the indentation {indentation} times there was still whitespace: {commandAsChars}");
            }
            
            if (indentation > _currentIndentation)
                throw new InvalidOperationException($"Invalid indentation in {line}");
            
            while (indentation < _currentIndentation)
                ExitScope();
                
            var command = ParseCommand(new string(commandAsChars));
            CurrentScope.Add(command);
            
            if (command is Repeat { MutCommands: var repeatScope })
                EnterScope(repeatScope);
            
        }
        return _programScope;
    }


    private ICommand ParseCommand(string line)
    {
        if (char.IsWhiteSpace(line, 0))
            throw new ArgumentException($"Invalid whitespace in command after repeatedly removing the indentation char: {line}");

        string[] words = line.Split(' ');

        if (words.Length == 0)
        {
            throw new ArgumentException("Empty command");
        }
        string commandName = words[0];
        string[] arguments = words[1..];
        ICommand command = commandName switch
        {
            "Move" => Move.Parse(arguments, logger),
            "Turn" => Turn.Parse(arguments, logger),
            "Repeat" => Repeat.Parse(arguments),
            _ => throw new ArgumentException($"Unknown command: {commandName}")
        };
        return command;
    }
}

public static class SpanHelpers
{
    public static unsafe Span<char> UnsafeMoveToSpan(this string? str)
    {
        if (str is null) return Span<char>.Empty;
        fixed (char* p = str) return new Span<char>(p, str.Length);
    }
}