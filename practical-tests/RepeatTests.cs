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

    public void ExecuteRepeat_Player(int times)
    {
        var command = Substitute.For<ICommand>();
        Repeat repeat = new Repeat(times, [command]);
        
        repeat.Execute(_player);
        command.Received().Execute(_player);
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
    [InlineData("10")]
    [InlineData("15")]
    public void Parse(string arguments)
    {
        Repeat repeat = Repeat.Parse([arguments, "times"]);

        int.TryParse(arguments, out int v);
        
        Assert.Equal(v, repeat.Times);
    }
    
    [Theory]
    [InlineData([])]
    [InlineData("15","13")]
    [InlineData("-10")]
    [InlineData("Een string")]
    [InlineData("Right")]
    [InlineData("Left")]
    public void Parse_Throws(params string[] args)
    {
        Action action = () => Repeat.Parse(args);

        Assert.Throws<ArgumentException>(action);
    }

    private string Output => _writer.ToString();
    
    [Theory]
    [InlineData(15)]
    [InlineData(300)]
    public void WriteHeaderTo(int times)
    {
        Repeat repeat = new Repeat(times);
        repeat.WriteHeaderTo(_writer);
        
        Assert.Equal($"Repeat {times} times{Environment.NewLine}", Output);
    }
}