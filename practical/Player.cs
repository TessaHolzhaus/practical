namespace Practical;

using System.Numerics;
using Subject;

public interface IPlayerObserver
{
    void OnMove(Coord position);
    void OnRotate(Direction direction);
}

public enum Direction{
    North,
    East,
    South,
    West
}

public readonly record struct Coord(int X, int Y)
{
    public static Coord operator +(Coord a, Coord b) => new(a.X + b.X, a.Y + b.Y);
    public static Coord operator -(Coord a, Coord b) => new(a.X - b.X, a.Y - b.Y);
    public static Coord operator *(int scalar, Coord a) => new(a.X * scalar, a.Y * scalar);
    public static Coord operator *(Coord a, int scalar) => scalar * a;
}

public class Player
{
    public Direction Direction
    {
        get;
        set
        {
            field = value;
            _subject.Notify(observer => observer.OnRotate(value));
        }
    }

    public Coord Position
    {
        get; 
        set
        {
            field = value;
            _subject.Notify(observer => observer.OnMove(value));
        }
    }
    
    private readonly Subject<IPlayerObserver> _subject;
    public ISubject<IPlayerObserver> Subject => _subject;

    public Player()
    {
        _subject = new Subject<IPlayerObserver>();
        Direction = Direction.East;
        Position = new Coord(0, 0);
    }
}