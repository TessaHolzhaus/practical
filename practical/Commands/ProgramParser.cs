using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Practical.Writing;

namespace Practical.Commands;

public class ProgramParser(List<ICommandParser> commandParsers, ILogger? logger = null)
{
    public ILogger? Logger { get; set; } = logger;
    public Indent Indent { get; set; } = new(' ', 4);

    private ProgramBuilder _programScope = null!;
    private Stack<ICommandBuilder> _scopesStack = null!;
    private int _currentIndentation = 0;

    private class ProgramBuilder : ICommandBuilder
    {
        public readonly List<ICommand> Commands = [];

        public bool TryAddCommand(ICommand command)
        {
            Commands.Add(command);
            return true;
        }

        public void UseLogger(ILogger? logger) => throw new InvalidOperationException();
        public bool TryOpenNextScope(ReadOnlySpan<char> header) => throw new InvalidOperationException();
        public ICommand Build() => throw new InvalidOperationException();
    }
    
    private void EnterScope(ICommandBuilder scope)
    {
        _scopesStack.Push(scope);
        _currentIndentation++;
    }

    private void ExitScope()
    {
        var finishedScope = _scopesStack.Pop();
        if (!CurrentScope.TryAddCommand(finishedScope.Build()))
            throw new UnreachableException("The parent scope should not be closed when closing one of its children");
        _currentIndentation--;
    }

    private ICommandBuilder CurrentScope => _scopesStack.TryPeek(out var result) ? result : _programScope;

    private void Reset()
    {
        _scopesStack = [];
        _programScope = new ProgramBuilder();
    }

    private void TransitionScope(int indentation, ReadOnlySpan<char> maybeScopeHeader)
    {
        if (indentation > _currentIndentation)
            throw new InvalidOperationException($"Invalid indentation before: {maybeScopeHeader}");

        while (indentation < _currentIndentation)
        {
            if (indentation == _currentIndentation - 1 && CurrentScope.TryOpenNextScope(maybeScopeHeader))
                return;
            ExitScope();
        }
    }
    
    public List<ICommand> ParseProgram(TextReader reader)
    {
        Reset();
        while (reader.ReadLine() is { } line)
        {
            var commandAsChars = Indent.TrimFromStart(line.UnsafeMoveToSpan(), out var indentation);
            if (commandAsChars.IsEmpty)
                continue;

            if (char.IsWhiteSpace(commandAsChars[0]))
            {
                if (commandAsChars.IsWhiteSpace()) continue;
                throw new InvalidOperationException($"After trimming the indentation {indentation} times there was still whitespace: {commandAsChars}");
            }
            
            TransitionScope(indentation, commandAsChars);
            
            if (!TryParseCommand(commandAsChars, out var builder))
                throw new InvalidOperationException($"The Following command is not registered: {commandAsChars}");
            
            builder.UseLogger(Logger);
            EnterScope(builder);
        }

        while (_currentIndentation > 0)
            ExitScope();
        
        return _programScope.Commands;
    }

    private bool TryParseCommand(ReadOnlySpan<char> commandAsChars, [MaybeNullWhen(false)] out ICommandBuilder builder)
    {
        foreach (var parser in commandParsers)
        {
            if (parser.TryParse(commandAsChars, out builder))
                return true;
        }
        
        builder = null;
        return false;
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