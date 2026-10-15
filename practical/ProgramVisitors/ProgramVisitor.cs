using Practical.Commands;

namespace Practical.ProgramVisitors;

public abstract class ProgramVisitor
{
    public void Visit(params IReadOnlyCollection<ICommand> commands)
    {
        foreach (var command in commands)
            command.Accept(this);
    }
    
    public abstract void VisitMove(Move move);
    public abstract void VisitTurn(Turn move);
    public abstract void VisitRepeat(Repeat repeat);
}