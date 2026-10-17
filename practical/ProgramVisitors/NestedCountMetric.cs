using Practical.Commands;

namespace Practical.ProgramVisitors;

public class NestedCountMetric : ProgramVisitor
{
    public int MaxNesting { get; private set; }
    private int _currentNesting = 0;
    
    private void EnterScope()
    {
        _currentNesting++;
        if (_currentNesting > MaxNesting) 
            MaxNesting = _currentNesting;
    }
    
    private void ExitScope()
    {
        _currentNesting--;
    }

    public override void VisitMove(Move move) {}
    public override void VisitTurn(Turn move) {}
    public override void VisitRepeat(Repeat repeat)
    {
        EnterScope();
        Visit(repeat.Commands);
        ExitScope();
    }

}