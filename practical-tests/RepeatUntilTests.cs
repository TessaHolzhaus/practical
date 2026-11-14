namespace PracticalTests;
using NSubstitute;
using Practical;
using Practical.Commands;
using Practical.ProgramVisitors;

public class RepeatUntilTests
{
    private readonly World _world = new(new Grid(new Field[,] { }));
    private readonly StringWriter _writer = new();

    //TODO: Create test grids
    
// public class RepeatUntilTestData : IEnumerable<object[]>
// {
//     public IEnumerator<object[]> GetEnumerator()
//     {
//         yield return new object[] { new WallAhead() };
//         yield return new object[] { new GridEdge() };
//     }
//
//     IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
// }

    public static IEnumerable<object[]> Conditions =>
    [
        [new WallAhead()],
        [new GridEdge()]
    ];
    
    //TODO: Write test for execution. Currently an issue with how the condition is accepted
    // [Fact] 
    // public void ExecuteRepeatUntil_WallAhead()
    // {
    //     var command = Substitute.For<ICommand>();
    //     ICondition wallAhead = new WallAhead();
    //     RepeatUntil repeatUntil = new RepeatUntil(wallAhead, [command]);
    //     
    //     repeatUntil.Execute(_world);
    //     command.Received().Execute(_world);
    // }

    [Fact]
    public void AcceptRepeatUntil_CallsVisitRepeatUntil()
    {
        var visitor = Substitute.For<ProgramVisitor>();
        var command = Substitute.For<ICommand>();
        var condition = Substitute.For<ICondition>();

        RepeatUntil repeatUntil = new RepeatUntil(condition, [command]);
        
        repeatUntil.Accept(visitor);
        visitor.Received(1).VisitRepeatUntil(repeatUntil);
    }

    [Theory]
    [InlineData("WallAhead")]
    [InlineData("GridEdge")]

    public void Parse(string arguments)
    {
        RepeatUntil repeatUntil = RepeatUntil.Parse([arguments]);

        if (arguments == "WallAhead")
            Assert.IsType<WallAhead>(repeatUntil.Condition);
        else
            Assert.IsType<GridEdge>(repeatUntil.Condition);
    }

    [Theory]
    [InlineData("15")]
    [InlineData("Hello")]
    [InlineData("WallAhead", "GridEdge")]
    [InlineData("Testing")]
    public void Parse_Throws(params string[] args)
    {
        Action action = () => RepeatUntil.Parse(args);

        Assert.Throws<ArgumentException>(action);
    }

    private string Output => _writer.ToString();
    
    [Theory] 
    [MemberData(nameof(Conditions))]
    public void WriteHeaderTo(ICondition condition)
    {
        RepeatUntil repeatUntil = new RepeatUntil(condition);
        repeatUntil.WriteHeaderTo(_writer);
        
        Assert.Equal($"RepeatUntil {condition}", Output);
    }
}