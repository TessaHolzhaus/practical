using System.Numerics;
using NSubstitute;
using Practical.ProgramVisitors;
using Practical.Commands;

namespace PracticalTests;
using Practical.Commands;
using Practical;

public class MoveTests
{
    private readonly World _world = new(new Grid(new Field[,]{}));
    private readonly StringWriter _writer = new();

    public static IEnumerable<object[]> Directions =>
    [
        [Direction.North],
        [Direction.East],
        [Direction.South],
        [Direction.West]
    ];
    //TODO: Replace execute test with 1 test that can go in every direction, similar to grid and wall error test?
    [Theory]
    [InlineData(0,0)]
    [InlineData(1,-1)]
    [InlineData(2, -2)]
    [InlineData(3,-3)]
    [InlineData(4,-4)]
    public void ExecuteMove_DirectionNorth_MovePlayer(int moveDist, int yCoord)
    {
        _world.Grid = new Grid(new [,]
        {
            {Field.Empty},
            {Field.Empty},
            {Field.Empty},
            {Field.Empty},
            {Field.Empty}
        });
        _world.Player.Direction = Direction.North;
        _world.Player.Position = new Coord(0, yCoord);
        Move move = new Move(moveDist);
        move.Execute(_world);
        Assert.Equal(new Coord(0, yCoord + moveDist), _world.Player.Position);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void ExecuteMove_DirectionEast_MovePlayer(int moveDist)
    {
        _world.Grid = new Grid(new [,]
        {
            {Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty}
        });
        _world.Player.Direction = Direction.East;
        Move move = new Move(moveDist);
        move.Execute(_world);
        Assert.Equal(new Coord(moveDist, 0), _world.Player.Position);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void ExecuteMove_DirectionSouth_MovePlayer(int moveDist)
    {
        _world.Grid = new Grid(new [,]
        {
            {Field.Empty},
            {Field.Empty},
            {Field.Empty},
            {Field.Empty},
            {Field.Empty}
        });
        _world.Player.Direction = Direction.South;
        Move move = new Move(moveDist);
        move.Execute(_world);
        Assert.Equal(new Coord(0, -moveDist), _world.Player.Position);
    }
    
    [Theory]
    [InlineData(0,0)]
    [InlineData(1,1)]
    [InlineData(2,2)]
    [InlineData(3,3)]
    [InlineData(4,4)]
    public void ExecuteMove_DirectionWest_MovePlayer(int moveDist, int xCoord)
    {
        _world.Grid = new Grid(new [,]
        {
            {Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty}
        });
        _world.Player.Direction = Direction.West;
        _world.Player.Position = new Coord(xCoord,0);
        Move move = new Move(moveDist);
        move.Execute(_world);
        Assert.Equal(new Coord(xCoord - moveDist, 0), _world.Player.Position);
    }
    
    [Theory]
    [MemberData(nameof(Directions))]
    public void ExecuteMove_GridError(Direction direction)
    {
        _world.Grid = new Grid(new [,]{{Field.Empty}});
        _world.Player.Direction = direction;
        Move move = new Move(1);
        Action action = () => move.Execute(_world);

        Assert.Throws<Exception>(action);
    }
    
    [Theory]
    [MemberData(nameof(Directions))]
    public void ExecuteMove_WallError(Direction direction)
    {
        _world.Grid = new Grid(new [,]
        {
            {Field.Wall,Field.Wall,Field.Wall},
            {Field.Wall,Field.Empty,Field.Wall},
            {Field.Wall,Field.Wall,Field.Wall}
        });
        _world.Player.Position = new Coord(1, 1);
        _world.Player.Direction = direction;
        Move move = new Move(1);
        Action action = () => move.Execute(_world);

        Assert.Throws<Exception>(action);
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