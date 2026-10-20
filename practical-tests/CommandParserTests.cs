using Practical.Commands;

namespace PracticalTests;

public class CommandParserTests
{
    private CommandParser _parser;

    public CommandParserTests()
    {
        _parser = new CommandParser();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("    ")]
    public void Parse_WhitespaceOnly_Throws(string input)
    {
        Assert.Equal([], RunParse(" "));
    } 
    
    [Theory]
    [InlineData(" f")]
    [InlineData("\th")]
    [InlineData("\t\t\txyz")]
    [InlineData("    a")]
    public void Parse_WhitespaceAndChar_Throws(string input)
    {
        Assert.Throws<InvalidOperationException>(() => RunParse(input));
    }
    
    [Theory]
    [InlineData("asds")]
    [InlineData("kyomh")]
    [InlineData("p")]
    public void Parse_InvalidCorrectIndent_Throws(string input)
    {
        Assert.Throws<ArgumentException>(() => RunParse(input));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(42)]
    [InlineData(-13)]
    public void Parse_Move_ReturnsMove(int amount)
    {
        var command = new Move(amount);
        AssertCommandParses(
            command,
            writer => command.WriteHeaderTo(writer),
            repeat => repeat.Distance
        );
    }
    
    [Theory]
    [InlineData(RotationDirection.Left)]
    [InlineData(RotationDirection.Right)]
    public void Parse_Turn_ReturnsMove(RotationDirection rotation)
    {
        var command = new Turn(rotation);
        AssertCommandParses(
            command,
            writer => command.WriteHeaderTo(writer),
            repeat => repeat.Rotation
        );
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(14)]
    public void Parse_EmptyRepeat_ReturnsRepeat(int times)
    {
        var command = new Repeat(times, []);
        AssertCommandParses(
            command,
            writer => command.WriteHeaderTo(writer),
            repeat => repeat.Times
        );
    }
    
    // Check Parse for None Empty repeat (aka only the body)

    private void AssertCommandParses<TCommand, TProp>(TCommand command, Action<TextWriter> writeTo, Func<TCommand, TProp> checkedProperty)
    {
        var writer = new StringWriter();
        writeTo(writer);
        var input = new StringReader(writer.ToString());
        
        if (_parser.CommandParse(input) is [TCommand parsed])
            Assert.Equal(checkedProperty(command), checkedProperty(parsed));
        else
            Assert.Fail();

    }
    
    
    private List<ICommand> RunParse(string input) => _parser.CommandParse(new StringReader(input)); 
}