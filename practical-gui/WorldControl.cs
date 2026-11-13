using Practical;
using System.ComponentModel;
using System.Diagnostics;

namespace practical_gui;

public class WorldControl : UserControl
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public World? World
    {
        get; 
        set
        {
            field = value;
            Invalidate();
        }
    }
    
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public required ICellRenderer CellRenderer { get; set; }
    
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public required IPlayerViewFactory PlayerViewFactory { get; set; }

    
    public WorldControl()
    {
        // TODO: inject world instead of creating it here (could also remove it and use property initialization, but that would call Invalidate())
        World = new World(new Grid(new [,]{
            {Field.Empty, Field.Empty, Field.Wall, Field.Wall, Field.Wall},
            {Field.Wall, Field.Empty, Field.Empty, Field.Wall, Field.Wall},
            {Field.Wall, Field.Wall, Field.Empty, Field.Wall, Field.Wall},
            {Field.Wall, Field.Wall, Field.Empty, Field.Wall, Field.Wall},
            {Field.Wall, Field.Wall, Field.Empty, Field.Empty, Field.Finish},
        }));
        
        SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true);
    }
    
    // TODO: refactor the drawing of the cells
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (World is null) return;

        int rows = World.Grid.Width;
        int cols = World.Grid.Height;
        if (rows <= 0 || cols <= 0) return;

        float cellWidth = (float)Width / cols;
        float cellHeight = (float)Height / rows;
        
        float cellSize = Math.Min(cellWidth, cellHeight);

        float offsetX = (ClientSize.Width - (cellSize * cols)) / 2f;
        float offsetY = (ClientSize.Height - (cellSize * rows)) / 2f;
        
        Graphics g = e.Graphics;

        float x = offsetX;
        float y = offsetY;
        
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                CellRenderer.Render(g, new RectangleF(x, y, cellSize, cellSize), World.Grid[r, c]);
                x += cellSize;
            }
            x = offsetX;
            y += cellSize;
        }
        
        PlayerViewFactory
            .Create(World.Player, () => Invalidate())
            .Render(g, new RectangleF(offsetX, offsetY, cellSize, cellSize));
    }
}

public interface ICellRenderer
{
    void Render(Graphics g, RectangleF bounds, Field field);
}

public class DefaultCellRenderer : ICellRenderer
{
    public void Render(Graphics g, RectangleF bounds, Field field)
    {
        var cellBrush = field switch
        {
            Field.Wall => Brushes.DarkSlateGray,
            Field.Empty => Brushes.White,
            Field.Finish => Brushes.LightGreen,
            _ => throw new UnreachableException()
        };

        g.FillRectangle(cellBrush, bounds);
        g.DrawRectangle(Pens.LightGray, bounds);
    }
}

public interface IPlayerView
{
    // TODO: make this not raw bounds (and Graphics?)
    public void Render(Graphics g, RectangleF bounds);
}

public interface IPlayerViewFactory
{
    IPlayerView Create(Player player, Action onChange);
}

public class PlayerPathView : IPlayerView
{
    private readonly Player _player;
    private readonly PlayerPath _path;
    
    public PlayerPathView(Player player, Action onChange)
    {
        _player = player;
        _path = new PlayerPath(onChange);
        _player.Subject.Subscribe(_path);
    }

    public void Render(Graphics g, RectangleF bounds)
    {
        // TODO: draw the path and then the player
        // TODO: player y should not need to be flipped (see another TODO for the same problem)
        bounds.Offset(bounds.Width * _player.Position.X, bounds.Height * -_player.Position.Y);
        var state = g.Save();

        try
        {
            g.TranslateTransform(
                bounds.X + bounds.Width / 2,
                bounds.Y + bounds.Height / 2
            );

            float angle = _player.Direction switch
            {
                Direction.North => 0,
                Direction.East => 90,
                Direction.South => 180,
                Direction.West => 270,
                _ => throw new ArgumentOutOfRangeException()
            };

            g.RotateTransform(angle);

            PointF[] points =
            [
                new(0, -bounds.Height * 0.4f),
                new(bounds.Width * 0.3f, bounds.Height * 0.3f),
                new(-bounds.Width * 0.3f, bounds.Height * 0.3f)
            ];

            g.FillPolygon(Brushes.Red, points);
        }
        finally
        {
            g.Restore(state);
        }
    }

    private class PlayerPath(Action OnChange) : IPlayerObserver
    {
        public List<Coord> Steps { get; set; } = [];

        public void OnRotate(Direction direction)
        {
            OnChange();
        }
        
        public void OnMove(Coord position)
        {
            Steps.Add(position);
            OnChange();
        }
    }

    public static IPlayerViewFactory Factory => new ViewFactory();
    private class ViewFactory : IPlayerViewFactory
    {
        public IPlayerView Create(Player player, Action onChange) => new PlayerPathView(player, onChange);
    }
}