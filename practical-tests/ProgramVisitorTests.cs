using Practical;
using Practical.Commands;
using Practical.ProgramVisitors;

namespace PracticalTests;

public class ProgramVisitorTests
{
    private class ProgramVisitorMock : ProgramVisitor
    {
        public override void VisitMove(Move move) => throw new InvalidOperationException();
        public override void VisitTurn(Turn move) => throw new InvalidOperationException();
        public override void VisitRepeat(Repeat repeat) => throw new InvalidOperationException();
    }

    internal class CommandAcceptMock : ICommand
    {
        public int TimesAccepted { get; private set; }
        public bool AcceptedOnce() => TimesAccepted == 1;
        
        public void Accept(ProgramVisitor visitor) => TimesAccepted++;
        public void Execute(Player p) => throw new InvalidOperationException();
    }
    
    private ProgramVisitor _visitor;
    
    public ProgramVisitorTests()
    {
        _visitor = new ProgramVisitorMock();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Visit_CallsAcceptOnCommands(int commandCount)
    {
        var commands = Enumerable.Range(0, commandCount)
            .Select(_ => new CommandAcceptMock())
            .ToList();

        _visitor.Visit(commands);
        
        Assert.All(commands, command => Assert.True(command.AcceptedOnce()));
    }
}