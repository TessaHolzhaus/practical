namespace PracticalTests;
using NSubstitute;
using Practical.ProgramVisitors;
using Practical.Commands;
using Practical;

public class RepeatTests
{
    private readonly Player _player = new();
    private readonly StringWriter _writer = new();

    [Theory]
    [InlineData(3)]
    [InlineData(4)]

    public void Execute_ExecutesCommandRepeatedly(int times)
    {
        var command = Substitute.For<ICommand>();
        Repeat repeat = new Repeat(times, [command]);
        
        repeat.Execute(_player);
        command.Received(times).Execute(_player);
    }

    [Fact]
    public void AcceptRepeat_CallsVisitRepeat()
    {
        var visitor = Substitute.For<ProgramVisitor>();
        var command = Substitute.For<ICommand>();
        Repeat repeat = new Repeat(5,[command]);
        
        repeat.Accept(visitor);
        visitor.Received(1).VisitRepeat(repeat);
    }
    
    [Theory]
    [InlineData("Repeat 10 times", 10)]
    [InlineData("Repeat 15 times", 15)]
    public void TryParse_ValidEmpty_ReturnsCorrectBuilder(string commandString, int expectedTimes)
    {
        if (!Repeat.TryParse(commandString, out var builder))
            Assert.Fail();

        var command = Assert.IsType<Repeat>(builder.Build());
        
        Assert.Equal(expectedTimes, command.Times);
        Assert.Equal([], command.Commands);
    }
    
    [Theory]
    [InlineData("" )]
    [InlineData("Repeat 15 13")]
    [InlineData("repeat 1 times" )]
    [InlineData("Repeat -10")]
    [InlineData("Repeat Een string")]
    [InlineData("Repeat 13 Strings")]
    public void TryParse_Invalid_ReturnsFalse(string args)
    {
        Assert.False(Move.TryParse(args, out _));
    }

    private string Output => _writer.ToString();
    
    [Theory]
    [InlineData(15)]
    [InlineData(300)]
    public void WriteHeaderTo(int times)
    {
        Repeat repeat = new Repeat(times);
        repeat.WriteHeaderTo(_writer);
        
        Assert.Equal($"Repeat {times} times", Output);
    }
}