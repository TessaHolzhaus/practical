using Practical.Commands;

namespace Practical.ProgramVisitors;

public class RepeatCountMetric : ProgramVisitor
{
    public int RepeatCount { get; private set; }

    public override void VisitMove(Move move) {}
    public override void VisitTurn(Turn move) {}

    public override void VisitRepeat(Repeat repeat)
    {
        RepeatCount++;
        Visit(repeat.Commands);
    }
}


public record Indent(char Char, int RepetitionsPerIndent = 1);

public static class IndentExtensions
{
    public static void WriteIndent(this TextWriter writer, Indent indent, int nesting)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(nesting);
        
        int count = indent.RepetitionsPerIndent * nesting;

        for (int i = 0; i < count; i++)
            writer.Write(indent.Char);
    }
}

// TODO: add a WriteTo methode to the command for a single source of truth
public class CommandStreamer : ProgramVisitor
{
    public TextWriter Writer { get; set; }
    public Indent Indent { get; set; } = new('\t');
    private int _indentationLevel = 0;
    
    public override void VisitMove(Move move)
    {
        Writer.WriteIndent(Indent, _indentationLevel);
        Writer.Write("Move ");
        Writer.WriteLine("[n]"); // TODO: replace with actual value
    }

    public override void VisitTurn(Turn move)
    {
        Writer.WriteIndent(Indent, _indentationLevel);
        Writer.Write("Turn ");
        Writer.WriteLine("[left/right]"); // TODO: replace with actual value

    }

    public override void VisitRepeat(Repeat repeat)
    {
        Writer.WriteIndent(Indent, _indentationLevel);
        Writer.Write("Repeat ");
        Writer.WriteLine("[n]"); // TODO: replace with actual value
        _indentationLevel++;
        Visit(repeat.Commands);
        _indentationLevel--;
    }
}