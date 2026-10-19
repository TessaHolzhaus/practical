using Practical.Commands;
using Practical.Writing;

namespace Practical.ProgramVisitors;

public class CommandStreamer : ProgramVisitor
{
    public TextWriter Writer { get; set; }
    public Indent Indent { get; set; } = new('\t');
    private int _indentationLevel = 0;

    public CommandStreamer(TextWriter writer)
    {
        Writer = writer;
    }

    public override void VisitMove(Move move)
    {
        Writer.WriteIndent(Indent, _indentationLevel);
        move.WriteHeaderTo(Writer);
        Writer.WriteLine();
    }

    public override void VisitTurn(Turn turn)
    {
        Writer.WriteIndent(Indent, _indentationLevel);
        turn.WriteHeaderTo(Writer);
        Writer.WriteLine();
    }

    public override void VisitRepeat(Repeat repeat)
    {
        Writer.WriteIndent(Indent, _indentationLevel);
        repeat.WriteHeaderTo(Writer);
        Writer.WriteLine();
        _indentationLevel++;
        Visit(repeat.Commands);
        _indentationLevel--;
    }
}