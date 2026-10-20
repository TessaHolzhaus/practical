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
    [InlineData(RotationDirection.Left, Direction.East, Direction.North)]
    [InlineData(RotationDirection.Right, Direction.East, Direction.South)]
    [InlineData(RotationDirection.Left, Direction.South, Direction.East)]
    [InlineData(RotationDirection.Right, Direction.South,Direction.West)]
    [InlineData(RotationDirection.Left, Direction.West, Direction.South)]
    [InlineData(RotationDirection.Right, Direction.West, Direction.North)]
    [InlineData(RotationDirection.Left, Direction.North,Direction.West)]
    [InlineData(RotationDirection.Right, Direction.North, Direction.East)]
    public void ExecuteTurn_East_Player(RotationDirection rotationDirection, Direction direction, Direction expected)
    {
        _player.Direction = direction;
        Turn turn = new Turn(rotationDirection);
        turn.Execute(_player);
        Assert.Equal(expected, _player.Direction);
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