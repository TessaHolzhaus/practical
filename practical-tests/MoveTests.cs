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
    public void ExecuteMove_DirectionNorth_MovePlayer(int moveDist)
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
    public void ExecuteMove_DirectionEast_MovePlayer(int moveDist)
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
    public void ExecuteMove_DirectionSouth_MovePlayer(int moveDist)
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
    public void ExecuteMove_DirectionWest_MovePlayer(int moveDist)
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
    [InlineData("15")]
    [InlineData("400")]
    [InlineData("-10")]
    public void Parse(params string[] arguments)
    {
        Move move = Move.Parse(arguments);

        var t = int.TryParse(arguments[0], out int v);
        
        Assert.Equal(v, move.Distance);
    }
    
    [Theory]
    [InlineData([])]
    [InlineData("15","13")]
    [InlineData("Een string")]
    public void Parse_Throws(params string[] args)
    {
        Action action = () => Move.Parse(args);

        Assert.Throws<ArgumentException>(action);
    }

    private string Output => _writer.ToString();
    
    [Theory]
    [InlineData(15)]
    [InlineData(-10)]
    [InlineData(500)]
    public void WriteHeaderTo(int dist)
    {
        Move move = new Move(dist);
        move.WriteHeaderTo(_writer);
        
        Assert.Equal($"Move {dist}", Output);
    }
}