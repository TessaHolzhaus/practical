using System.Numerics;
using NSubstitute;
using Practical.ProgramVisitors;

namespace PracticalTests;
using Practical.Commands;
using Practical;
public class MoveTests
{
    private readonly Player _player = new();
    private readonly StringWriter _writer = new();

    [Theory]
    [InlineData(10)]
    [InlineData(500)]
    [InlineData(-10)]
    [InlineData(0)]
    public void Execute_DirectionNorth_MovePlayer(int moveDist)
    {
        _player.Direction = Direction.North;
        Move move = new Move(moveDist);
        move.Execute(_player);
        Assert.Equal(new Vector2(0, moveDist), _player.Position);
    }
    
    [Theory]
    [InlineData(10)]
    [InlineData(500)]
    [InlineData(-10)]
    [InlineData(0)]
    public void Execute_DirectionEast_MovePlayer(int moveDist)
    {
        _player.Direction = Direction.East;
        Move move = new Move(moveDist);
        move.Execute(_player);
        Assert.Equal(new Vector2(moveDist, 0), _player.Position);
    }
    
    [Theory]
    [InlineData(10)]
    [InlineData(500)]
    [InlineData(-10)]
    [InlineData(0)]
    public void Execute_DirectionSouth_MovePlayer(int moveDist)
    {
        _player.Direction = Direction.South;
        Move move = new Move(moveDist);
        move.Execute(_player);
        Assert.Equal(new Vector2(0, -moveDist), _player.Position);
    }
    
    [Theory]
    [InlineData(10)]
    [InlineData(500)]
    [InlineData(-10)]
    [InlineData(0)]
    public void Execute_DirectionWest_MovePlayer(int moveDist)
    {
        _player.Direction = Direction.West;
        Move move = new Move(moveDist);
        move.Execute(_player);
        Assert.Equal(new Vector2(-moveDist, 0), _player.Position);
    }
    
    [Fact]
    public void AcceptMove_CallsVisitMove()
    {
        var visitor = Substitute.For<ProgramVisitor>();
        Move move = new Move(10);
        
        move.Accept(visitor);
        visitor.Received(1).VisitMove(move);
    }
    
    [Theory]
    [InlineData("Move 15", 15)]
    [InlineData("Move 400", 400)]
    [InlineData("Move -10", -10)]
    public void TryParse_Valid_ReturnsCorrectBuilder(string commandString, int expectedDistance)
    {
        if (!Move.TryParse(commandString, out var builder))
            Assert.Fail();

        var command = Assert.IsType<Move>(builder.Build());
        
        Assert.Equal(expectedDistance, command.Distance);
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
        Assert.False(Move.TryParse(args, out _));
    }

    private string Output => _writer.ToString();
    
    [Theory]
    [InlineData(15)]
    [InlineData(-10)]
    [InlineData(500)]
    public void WriteHeaderTo(int dist)
    {
        var move = new Move(dist);
        move.WriteHeaderTo(_writer);
        
        Assert.Equal($"Move {dist}", Output);
    }
}