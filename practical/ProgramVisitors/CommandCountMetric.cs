using Practical.Commands;

namespace Practical.ProgramVisitors;

public class CommandCountMetric : ProgramVisitor
{
    public int CommandCount { get; private set; }
    
    public override void VisitMove(Move move)
        => CommandCount++;

    public override void VisitTurn(Turn turn)
        => CommandCount++;

    public override void VisitRepeat(Repeat repeat)
    {
        CommandCount++;
        Visit(repeat.Commands);      
    }
    public override void VisitRepeatUntil(RepeatUntil repeatUntil)
    {
        CommandCount++;
        Visit(repeatUntil.Commands);
    }
}