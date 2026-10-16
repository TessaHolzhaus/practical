using Practical.Commands;

namespace Practical.ProgramVisitors;

public class NestedCountMetric : ProgramVisitor
{
    public int MaxNesting { get; private set; }
    private int _currentNesting = 0;
    
    public override void VisitMove(Move move) {}
    public override void VisitTurn(Turn move) {}
    public override void VisitRepeat(Repeat repeat)
    {
        _currentNesting++;
        if (_currentNesting > MaxNesting) MaxNesting = _currentNesting;
        Visit(repeat.Commands);
        _currentNesting--;
    }
}