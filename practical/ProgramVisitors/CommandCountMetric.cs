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
        foreach (var command in repeat.Commands)
            command.Accept(this);       
    }
}