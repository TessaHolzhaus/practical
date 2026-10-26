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
    public void Execute_TurnsPlayer(RotationDirection rotationDirection, Direction direction, Direction expected)
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
    [InlineData("Turn left", RotationDirection.Left)]
    [InlineData("Turn right", RotationDirection.Right)]
    public void TryParse_Valid_ReturnsCorrectBuilder(string commandString, RotationDirection direction)
    {
        if (!Turn.TryParse(commandString, out var builder))
            Assert.Fail();

        var command = Assert.IsType<Turn>(builder.Build());
        
        Assert.Equal(direction, command.Rotation);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("23 42")]
    [InlineData("move 400")]
    [InlineData("Move")]
    [InlineData("Move 15 13")]
    [InlineData("Move Een string")]
    public void TryParse_Invalid_ReturnsFalse(string args)
    {
        Assert.False(Move.TryParse(args, out var builder));
        Assert.Null(builder);
    }
    
    [Theory]
    [InlineData(RotationDirection.Right)]
    [InlineData(RotationDirection.Left)]
    public void WriteHeaderTo(RotationDirection direction)
    {
        Turn turn =  new Turn(direction);
        turn.WriteHeaderTo(_writer);
        
        Assert.Equal($"Turn {direction.ToString().ToLower()}", Output);
    }
    
    private string Output => _writer.ToString();

}