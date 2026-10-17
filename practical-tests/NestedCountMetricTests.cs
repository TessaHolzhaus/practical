using Practical.Commands;
using Practical.ProgramVisitors;
using System.Reflection;

namespace PracticalTests;

public class NestedCountMetricTests : ProgramVisitorRecursiveSubclassTests
{
    private readonly NestedCountMetric _metric;

    public NestedCountMetricTests()
    {
        _metric = new NestedCountMetric();
    }
    
    protected override ProgramVisitor Visitor => _metric;
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void VisitMove_PreservesRepeatCount(int visits)
    {
        var commands = Enumerable.Range(0, visits).Select(_ => new Move(5)).ToList();

        AssertMaxNestingUnchanged(commands, command => _metric.VisitMove(command)); 
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void VisitTurn_PreservesRepeatCount(int visits)
    {
        var commands = Enumerable.Range(0, visits).Select(_ => new Turn(RotationDirection.Left)).ToList();

        AssertMaxNestingUnchanged(commands, command => _metric.VisitTurn(command)); 
    }
    
    [Fact]
    public void VisitRepeat_AtMaxNesting_IncrementsMaxNesting()
    {
        var command = new Repeat(5, []);
    
        _metric.VisitRepeat(command);
        
        Assert.Equal(1, _metric.MaxNesting);
    }
    

    private static readonly Lazy<PropertyInfo> MaxNestingProperty = new(() =>
        typeof(NestedCountMetric).GetProperty(nameof(NestedCountMetric.MaxNesting), BindingFlags.Public | BindingFlags.Instance) 
        ?? throw new InvalidOperationException("Property CurrentNesting niet gevonden."));

    
    [Fact]
    public void VisitRepeat_BelowMaxNesting_PreservesMaxNesting()
    {
        var command = new Repeat(5, []);
    
        MaxNestingProperty.Value.SetValue(_metric, 10);
        
        _metric.VisitRepeat(command);
        
        Assert.Equal(10, _metric.MaxNesting);
    }
    
    private void AssertMaxNestingUnchanged<TCommand>(IReadOnlyCollection<TCommand> commands, Action<TCommand> visitAction)
    {
        foreach (var command in commands)
        {
            visitAction(command);
        }
        
        Assert.Equal(0, _metric.MaxNesting);
    }
}