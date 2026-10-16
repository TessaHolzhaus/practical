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