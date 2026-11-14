using Practical;
using Practical.Commands;
using Practical.ProgramVisitors;
using NSubstitute;
namespace PracticalTests;

public class GoToTests
{
    private readonly World _world = new(new Grid(new [,]{
        {Field.Empty, Field.Empty, Field.Wall, Field.Wall, Field.Wall},
        {Field.Wall, Field.Empty, Field.Empty, Field.Wall, Field.Wall},
        {Field.Wall, Field.Wall, Field.Empty, Field.Wall, Field.Wall},
        {Field.Wall, Field.Wall, Field.Empty, Field.Wall, Field.Wall},
        {Field.Wall, Field.Wall, Field.Empty, Field.Empty, Field.Finish},
    }));
    private readonly StringWriter _writer = new();

    [Theory]
    [InlineData(0,0)]
    [InlineData(2,-1)]
    [InlineData(1,0)]
    [InlineData(2,-2)]
    [InlineData(0,-2)]
    public void ExecuteGoTo(int x, int y)
    {
        _world.Grid = new Grid(new[,]
        {
            { Field.Empty, Field.Empty, Field.Empty },
            { Field.Empty, Field.Empty, Field.Empty },
            { Field.Empty, Field.Empty, Field.Empty },
        });
        Coord coords = new Coord(x, y);
        GoTo goTo = new GoTo(coords);
        goTo.Execute(_world);

        Assert.Equal(coords, _world.Player.Position);
    }

    [Theory]
    [InlineData(1,0)]
    [InlineData(1,-1)]
    [InlineData(0,-1)]
    [InlineData(0,1)]
    public void ExecuteGoTo_GridError(int x, int y)
    {
        _world.Grid = new Grid(new [,]{{Field.Empty}});
        Coord coords = new Coord(x, y);
        GoTo goTo = new GoTo(coords);
        
        Action action = () => goTo.Execute(_world);
        Assert.Throws<Exception> (action);
    }
    
    [Theory]
    [InlineData(1,0)]
    [InlineData(1,-1)]
    [InlineData(0,-1)]
    [InlineData(0,1)]
    public void ExecuteGoTo_WallError(int x, int y)
    {
        _world.Grid = new Grid(new[,]
        {
            { Field.Empty, Field.Wall, Field.Wall },
            { Field.Wall, Field.Wall, Field.Wall },
            { Field.Wall, Field.Wall, Field.Wall },
        });
        Coord coords = new Coord(x, y);
        GoTo goTo = new GoTo(coords);
        
        Action action = () => goTo.Execute(_world);
        Assert.Throws<Exception> (action);
    }
    
    [Fact]
    public void AcceptGoTo_CallsVisitGoTo()
    {
        Coord coords = new Coord(2, -2);
        var visitor = Substitute.For<ProgramVisitor>();
        GoTo goTo = new GoTo(coords);
        
        goTo.Accept(visitor);
        visitor.Received(1).VisitGoTo(goTo);
    }
    
    [Theory]
    [InlineData("(15,12)")]
    [InlineData("(100 , 23)")]
    [InlineData("(400,34)")]
    [InlineData("(-10,0)")]
    public void Parse(params string[] arguments)
    {
        GoTo goTo = GoTo.Parse(arguments);

        string[] values = arguments[0].Trim('(', ')').Split(',');
        var t = int.TryParse(values[0], out int x);
        var s = int.TryParse(values[1], out int y);
        Coord coord = new Coord(x, y);
        
        Assert.Equal(coord, goTo.Coords);
    }
    
    [Theory]
    [InlineData([])]
    [InlineData("15","13")]
    [InlineData("Een string")]
    [InlineData("(15,13)", "13")]

    public void Parse_Throws(params string[] args)
    {
        Action action = () => GoTo.Parse(args);

        Assert.Throws<ArgumentException>(action);
    }
    
    public static IEnumerable<object[]> Coords =>
    [
        [new Coord(15,12)],
        [new Coord(100,23)],
        [new Coord(400,34)],
        [new Coord(-10,0)]
    ];
    
    private string Output => _writer.ToString();
    
    [Theory]
    [MemberData(nameof(Coords))]
    public void WriteHeaderTo(Coord coord)
    {
        GoTo goTo = new GoTo(coord);
        goTo.WriteHeaderTo(_writer);
        
        Assert.Equal($"GoTo ({coord.X},{coord.Y})", Output);
    }
}