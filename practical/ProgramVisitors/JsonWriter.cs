using Practical.Commands;
using Practical.Writing;

namespace Practical.ProgramVisitors;

public class JsonWriter : ProgramVisitor
{
    private readonly Stack<char> _closingBrackets;
    private int _deepestItemIndent;
    private readonly TextWriter _writer;

    public JsonWriter(TextWriter writer)
    {
        _closingBrackets = new Stack<char>();
        _deepestItemIndent = -1;
        _writer = writer;
    }

    private int Indentation => _closingBrackets.Count;
    private bool InList => _closingBrackets.TryPeek(out var c) && c == ']';
    
    public Indent IndentationChar { get; set; } = new(' ', 4);
        
    private void WritePropertyName(string name)
    {
        if (_deepestItemIndent == Indentation)
            _writer.WriteLine(',');
        WriteIndentation();
        _writer.Write('"');
        _writer.Write(name);
        _writer.Write('"');
        _writer.Write(':');
        _writer.Write(' ');
    }

    private void WriteIndentation()
    {
        Span<char> indent = stackalloc char[Indentation * IndentationChar.RepetitionsPerIndent];
        indent.Fill(IndentationChar.Char);
        _writer.Write(indent);
    }
    
    private void WriteValue(string value)
    {
        _writer.Write(value);
        _deepestItemIndent = Indentation;
    }

    protected void Open(char open, char close)
    {
        if (InList)
        {
            if (_deepestItemIndent == Indentation)
                _writer.WriteLine(',');
            WriteIndentation();
        }
        _writer.WriteLine(open);
        _closingBrackets.Push(close);
    }
    
    public void Close()
    {
        var closing = _closingBrackets.Pop();
        _writer.WriteLine();
        WriteIndentation();
        _writer.Write(closing);
        _deepestItemIndent = Indentation;
    }

    private void OpenObject() => Open('{', '}');
    private void OpenList() => Open('[', ']');

    private void OpenCommand(string command)
    {
        OpenObject();
        WritePropertyName("command");
        WriteValue(command);
    }
    
    public override void Visit(params IReadOnlyCollection<ICommand> commands)
    {
        OpenList();
        base.Visit(commands);
        Close();
    }

    public override void VisitMove(Move move)
    {
        OpenCommand("move");
        WritePropertyName("distance");
        WriteValue(move.Distance.ToString());
        Close();
    }

    public override void VisitTurn(Turn turn)
    {
        OpenCommand("turn");
        WritePropertyName("rotation");
        WriteValue(turn.Rotation.ToString().ToLower());
        Close();
    }

    public override void VisitRepeat(Repeat repeat)
    {
        OpenCommand("repeat");
        WritePropertyName("times");
        WriteValue(repeat.Times.ToString());
        WritePropertyName("commands");
        Visit(repeat.Commands);
        Close();
    }

    public override void VisitRepeatUntil(RepeatUntil repeatUntil)
    {
        OpenCommand("repeatUntil");
        WritePropertyName("condition");
        WriteValue(repeatUntil.Condition.ToString()!);
        WritePropertyName("commands");
        Visit(repeatUntil.Commands);
        Close();
    }

    public override void VisitGoTo(GoTo goTo)
    {
        WritePropertyName("goto");
        WriteValue(goTo.Coords.ToString());
    }
}