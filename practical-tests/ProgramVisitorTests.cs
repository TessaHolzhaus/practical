using Practical;
using Practical.Commands;
using Practical.ProgramVisitors;
using NSubstitute;

namespace PracticalTests;

public class ProgramVisitorTests
{
    internal class CommandAcceptMock : ICommand
    {
        public int TimesAccepted { get; private set; }
        public bool AcceptedOnce() => TimesAccepted == 1;
        
        public void Accept(ProgramVisitor visitor) => TimesAccepted++;
        public void Execute(Player p) => throw new InvalidOperationException();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Visit_CallsAcceptOnCommands(int commandCount)
    {
        var commands = Enumerable.Range(0, commandCount)
            .Select(_ => Substitute.For<ICommand>())
            .ToList();
        
        var visitor = Substitute.For<ProgramVisitor>();

        visitor.Visit(commands);
        
        Assert.All(commands, command => command.Received(1).Accept(Arg.Any<ProgramVisitor>()));
        
    }
}