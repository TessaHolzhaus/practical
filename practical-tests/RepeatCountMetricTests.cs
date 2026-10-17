using NSubstitute;
using Practical.Commands;
using Practical.ProgramVisitors;

namespace PracticalTests;

public class RepeatCountMetricTests
{
    private RepeatCountMetric _metric;
    
    public RepeatCountMetricTests()
    {
        _metric = new RepeatCountMetric();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void VisitMove_PreservesRepeatCount(int visits)
    {
        var commands = Enumerable.Range(0, visits).Select(_ => new Move(5)).ToList();

        AsserRepeatCountUnchanged(commands, command => _metric.VisitMove(command)); 
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void VisitTurn_PreservesRepeatCount(int visits)
    {
        var commands = Enumerable.Range(0, visits).Select(_ => new Turn(RotationDirection.Left)).ToList();

        AsserRepeatCountUnchanged(commands, command => _metric.VisitTurn(command)); 
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
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void VisitRepeat_CallsAcceptOnEachBodyCommand(int commandCount)
    {
        var commands = Enumerable.Range(0, commandCount)
            .Select(_ => Substitute.For<ICommand>())
            .ToList();

        var repeatCommand = new Repeat(3, commands);
        
        _metric.VisitRepeat(repeatCommand);
        
        Assert.All(commands, command => command.Received(1).Accept(Arg.Any<ProgramVisitor>()));
    }
    

    private void AsserRepeatCountUnchanged<TCommand>(IReadOnlyCollection<TCommand> commands, Action<TCommand> visitAction)
    {
        foreach (var command in commands)
        {
            visitAction(command);
        }
        
        Assert.Equal(0, _metric.RepeatCount);
    }
}