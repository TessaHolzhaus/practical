using Practical.Commands;
using Practical.ProgramVisitors;

namespace PracticalTests;

public class CommandCountMetricTests
{
    private CommandCountMetric _metric;

    public CommandCountMetricTests()
    {
        _metric = new CommandCountMetric();
    }
    
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void VisitMove_IncrementsCommandCount(int visits)
    {
        var commands = Enumerable.Range(0, visits).Select(_ => new Move()).ToList();
        AssertCommandIncrementsCount(commands, command => _metric.VisitMove(command));
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void VisitTurn_IncrementsCommandCount(int visits)
    {
        var commands = Enumerable.Range(0, visits).Select(_ => new Turn()).ToList();
        AssertCommandIncrementsCount(commands, command => _metric.VisitTurn(command));
    }
    
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void VisitRepeat_IncrementsCommandCount(int visits) // TODO: repeat constructor does not exist so the child increments throw (and are also not tested by this) 
    {
        var commands = Enumerable.Range(0, visits).Select(_ => new Repeat()).ToList();
        AssertCommandIncrementsCount(commands, command => _metric.VisitRepeat(command));
    }
    
    private void AssertCommandIncrementsCount<TCommand>(IReadOnlyCollection<TCommand> commands, Action<TCommand> visitAction)
    {
        foreach (var command in commands)
        {
            visitAction(command);
        }
        
        Assert.Equal(commands.Count, _metric.CommandCount);
    }
}