using Practical.Commands;
using Practical.ProgramVisitors;
using NSubstitute;

namespace PracticalTests;

public class ProgramVisitorTests
{
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
        
        ProgramVisitorAssert.AcceptCalledOnceForAll(commands);
    }
}

public abstract class ProgramVisitorRecursiveSubclassTests
{
    protected abstract ProgramVisitor Visitor { get; }
 
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void VisitRepeat_CallsAcceptOnCommands(int commandCount)
    {
        var commands = Enumerable.Range(0, commandCount)
            .Select(_ => Substitute.For<ICommand>())
            .ToList();
        
        var repeatCommand = new Repeat(3, commands);
        
        Visitor.VisitRepeat(repeatCommand);
        
        ProgramVisitorAssert.AcceptCalledOnceForAll(commands);
    }
}

public class ProgramVisitorAssert
{
    public static void AcceptCalledOnceForAll(params IReadOnlyCollection<ICommand> commands)
        => Assert.All(commands, AcceptCalledOnce);
    
    public static void AcceptCalledOnce(ICommand command)
        => command.Received(1).Accept(Arg.Any<ProgramVisitor>());
    
    public static void RepeatCallsAcceptOnceForAll(ProgramVisitor visitor, int commandCount)
    {
        var commands = Enumerable.Range(0, commandCount)
            .Select(_ => Substitute.For<ICommand>())
            .ToList();
        
        var repeatCommand = new Repeat(3, commands);
        
        
        visitor.VisitRepeat(repeatCommand);

        AcceptCalledOnceForAll(commands);
    }
}