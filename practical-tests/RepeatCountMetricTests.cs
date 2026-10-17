using Practical.Commands;
using Practical.ProgramVisitors;

namespace PracticalTests;

public class RepeatCountMetricTests : ProgramVisitorRecursiveSubclassTests
{
    private readonly RepeatCountMetric _metric;
    
    public RepeatCountMetricTests()
    {
        _metric = new RepeatCountMetric();
    }

    protected override ProgramVisitor Visitor => _metric;
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void VisitMove_PreservesRepeatCount(int visits)
    {
        var commands = Enumerable.Range(0, visits).Select(_ => new Move(5)).ToList();

        AssertRepeatCountUnchanged(commands, command => _metric.VisitMove(command)); 
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void VisitTurn_PreservesRepeatCount(int visits)
    {
        var commands = Enumerable.Range(0, visits).Select(_ => new Turn(RotationDirection.Left)).ToList();

        AssertRepeatCountUnchanged(commands, command => _metric.VisitTurn(command)); 
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void VisitRepeat_WithEmptyBody_IncrementsRepeatCount(int visits)
    {
        var commands = Enumerable.Range(0, visits)
            .Select(i => new Repeat(5, []))
            .ToList();
        
        foreach (var command in commands) 
            _metric.VisitRepeat(command);
        
        Assert.Equal(commands.Count, _metric.RepeatCount);
    }

    private void AssertRepeatCountUnchanged<TCommand>(IReadOnlyCollection<TCommand> commands, Action<TCommand> visitAction)
    {
        foreach (var command in commands)
        {
            visitAction(command);
        }
        
        Assert.Equal(0, _metric.RepeatCount);
    }
}