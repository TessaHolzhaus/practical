using System.Numerics;
using NSubstitute;
using Practical.ProgramVisitors;

namespace PracticalTests;
using Practical.Commands;
using Practical;

public class TurnTests
{

    private readonly Player _player = new();
    private readonly StringWriter _writer = new();

    [Theory]
    [InlineData(RotationDirection.Left)]
    [InlineData(RotationDirection.Right)]

    public void ExecuteTurn_East_Player(RotationDirection direction)
    {
        _player.Direction = Direction.East;
        Turn turn = new Turn(direction);
        turn.Execute(_player);
        Assert.Equal(direction is RotationDirection.Left ? Direction.North : Direction.South, _player.Direction);
    }

    [Fact]
    public void AcceptTurn_CallsVisitTurn()
    {
        var visitor = Substitute.For<ProgramVisitor>();
        Turn turn = new Turn(RotationDirection.Left);
        
        turn.Accept(visitor);
        visitor.Received(1).VisitTurn(turn);
    }
    
    [Theory]
    [InlineData("left")]
    [InlineData("right")]
    public void Parse(string arguments)
    {
        Turn turn = Turn.Parse([arguments]);

        Enum.TryParse(arguments, true, out RotationDirection v);
        
        Assert.Equal(v, turn.Rotation);
    }
    
    [Theory]
    [InlineData([])]
    [InlineData("15","13")]
    [InlineData("Een string")]
    [InlineData("Right")]
    [InlineData("Left")]
    public void Parse_Throws(params string[] args)
    {
        Action action = () => Turn.Parse(args);

        Assert.Throws<ArgumentException>(action);
    }

    private string Output => _writer.ToString();
    
    [Theory]
    [InlineData(RotationDirection.Right)]
    [InlineData(RotationDirection.Left)]
    public void WriteHeaderTo(RotationDirection direction)
    {
        Turn turn =  new Turn(direction);
        turn.WriteHeaderTo(_writer);
        
        Assert.Equal($"Turn {direction.ToString().ToLower()}", Output);
    }
}