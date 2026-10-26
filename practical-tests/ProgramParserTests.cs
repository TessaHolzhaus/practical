using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using Practical;
using Practical.Commands;

namespace PracticalTests;

public class ProgramParserTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("    ")]
    public void ParseProgram_WhitespaceOnly_ReturnsEmpty(string input)
    {
        var parser = new ProgramParser([]);
        
        Assert.Equal([], parser.ParseProgram(new StringReader(input)));
    } 
    
    [Theory]
    [InlineData(" f")]
    [InlineData("\th")]
    [InlineData("\t\t\txyz")]
    [InlineData("    a")]
    public void ParseProgram_WhitespaceAndUnknownCommand_Throws(string input)
    {
        var parser = new ProgramParser([]);
        Assert.Throws<InvalidOperationException>(() => parser.ParseProgram(new StringReader(input)));
    }
    
    [Theory]
    [InlineData("asds")]
    [InlineData("kyomh")]
    [InlineData("p")]
    public void ParseProgram_InvalidWithCorrectIndent_Throws(string input)
    {
        var parser = new ProgramParser([]);
        Assert.Throws<InvalidOperationException>(() => parser.ParseProgram(new StringReader(input)));
    }

    private delegate bool TryParseDelegate(ReadOnlySpan<char> input, out ICommandBuilder? commandBuilder);
    private class CommandParserMock(TryParseDelegate tryParse) : ICommandParser
    {
        public int Calls { get; private set; }
        public bool TryParse(ReadOnlySpan<char> line, [MaybeNullWhen(false)] out ICommandBuilder builder)
        {
            Calls++;
            return tryParse(line, out builder);
        }
        
        public static CommandParserMock ReturnsTrueWith(ICommandBuilder builder) => new(
            (_, out commandBuilder) =>
            {
                commandBuilder = Substitute.For<ICommandBuilder>();
                return true;
            }
        );

        public static CommandParserMock ReturnsFalse() => new(
            (_, out builder) =>
            {
                builder = null;
                return false;
            }
        );
    }

    [Fact]
    public void ParseProgram_StopsParsingOnFirstMatch()
    {
        var everyCommandParser = CommandParserMock.ReturnsTrueWith(Substitute.For<ICommandBuilder>());
        var otherCommandParser = CommandParserMock.ReturnsTrueWith(Substitute.For<ICommandBuilder>());

        var parser = new ProgramParser([everyCommandParser, otherCommandParser]);
        
        parser.ParseProgram(new StringReader("something"));
        
        Assert.Equal(1, everyCommandParser.Calls);
        Assert.Equal(0, otherCommandParser.Calls);
    }
    
    
    [Fact]
    public void ParseProgram_ContinuesParsingOnFirstMiss()
    {
        var firstCommandParser = CommandParserMock.ReturnsFalse();
        var otherCommandParser = CommandParserMock.ReturnsTrueWith(Substitute.For<ICommandBuilder>());

        var parser = new ProgramParser([firstCommandParser, otherCommandParser]);
        
        parser.ParseProgram(new StringReader("something"));
        
        Assert.Equal(1, firstCommandParser.Calls);
        Assert.Equal(1, otherCommandParser.Calls);
    }
    
    [Fact]
    public void ParseProgram_ThrowsOnAllMiss()
    {
        var firstCommandParser = CommandParserMock.ReturnsFalse();
        var otherCommandParser = CommandParserMock.ReturnsFalse();

        var parser = new ProgramParser([firstCommandParser, otherCommandParser]);
        
        Assert.Throws<InvalidOperationException>(
            () => parser.ParseProgram(new StringReader("something"))
        );
        Assert.Equal(1, firstCommandParser.Calls);
        Assert.Equal(1, otherCommandParser.Calls);
    }
}