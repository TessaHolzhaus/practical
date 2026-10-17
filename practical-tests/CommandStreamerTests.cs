using NSubstitute;
using Practical.Commands;
using Practical.ProgramVisitors;

namespace PracticalTests;

public class CommandStreamerTests : ProgramVisitorRecursiveSubclassTests
{
    private readonly CommandStreamer _streamer;
    private readonly StringWriter _writer;

    public string Output => _writer.ToString();
    
    public CommandStreamerTests()
    {
        _writer = new StringWriter();
        _streamer = new CommandStreamer(_writer);
    }
    
    protected override ProgramVisitor Visitor => _streamer;

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(42)]
    public void VisitMove_WritesCommand(int steps)
    {
        var command = new Move(steps);
        
        _streamer.VisitMove(command);
        
        Assert.Equal($"Move {steps}{Environment.NewLine}", Output);
    }
    
    [Theory]
    [InlineData(RotationDirection.Left)]
    [InlineData(RotationDirection.Right)]
    public void VisitTurn_WritesCommand(RotationDirection rotation)
    {
        var command = new Turn(rotation);
        
        _streamer.VisitTurn(command);
        
        Assert.Equal($"Turn {rotation}{Environment.NewLine}", Output); // Do we want the rotation segment to be the same as RotationDirection.ToString()? Maybe we want lower case?
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(42)]
    public void VisitRepeat_WritesRepeatHeader(int times)
    {
        var command = new Repeat(times, []);
        
        _streamer.VisitRepeat(command);
        
        Assert.Equal($"Repeat {times} times{Environment.NewLine}", Output); // Do we want the rotation segment to be the same as RotationDirection.ToString()? Maybe we want lower case?
    }

    public static IEnumerable<object[]> RepeatBodies() =>
    [
        [new List<ICommand> { new Move(10) }],
        [new List<ICommand> { new Move(10), new Turn(RotationDirection.Left) }],
        [new List<ICommand> { new Repeat(5, [new Move(10)]) }]
    ];
    
    [Theory]
    [MemberData(nameof(RepeatBodies))]
    public void VisitRepeat_IndentsRepeatBody(List<ICommand> commands)
    {
        var command = new Repeat(5, commands);
        
        _streamer.VisitRepeat(command);

        var bodyLines = Output
            .Split(Environment.NewLine)
            .Skip(1)
            .SkipLast(1); // Skip the last empty line resulting from Split, but only the last one in case the commands in the body result in string.Empty
        
        Assert.All(bodyLines, line => Assert.StartsWith(_streamer.Indent.ToString(), line));
    }
}