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
    public ICellRenderer CellRenderer { get; set; }
    
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IPlayerViewFactory PlayerViewFactory { get; set; }

    public IPlayerView PlayerView;
    
    public WorldControl(IPlayerViewFactory playerViewFactory, ICellRenderer cellRenderer)
    {
        // TODO: inject world instead of creating it here (could also remove it and use property initialization, but that would call Invalidate())
        World = World.Create();
        
        SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true);
        
        CellRenderer = cellRenderer;
        PlayerViewFactory = playerViewFactory;
        
        ResetPlayer();
    }

    public void ResetPlayer()
    {
        if (World is null) return;
        
        World.Player.Direction = Direction.East;
        World.Player.Position = new Coord(0, 0);
        
        PlayerView = PlayerViewFactory
            .Create(World.Player, () =>
            {
                if (World is null) return;
                if (World.Grid[World.Player.Position.X, -World.Player.Position.Y] is Field.Finish)
                    MessageBox.Show("Finished!", "Congratulations, you reached the finish!", MessageBoxButtons.OK);
                Invalidate();
            });
        
        Invalidate();
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
        
        PlayerView.Render(g, new RectangleF(offsetX, offsetY, cellSize, cellSize));
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
        _path = new PlayerPath(player.Position, onChange);
        _player.Subject.Subscribe(_path);
    }

    public void Render(Graphics g, RectangleF bounds)
    {
        var steps = _path.Steps.Select(step => 
            new PointF(bounds.X + (bounds.Width * (step.X + 0.5f)), bounds.Y + (bounds.Height * (-step.Y + 0.5f)))
        ).ToArray();
        
        if (steps.Length >= 2)
            g.DrawLines(Pens.Red, steps);
        
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

    private class PlayerPath(Coord startingAt, Action onChange) : IPlayerObserver
    {
        public List<Coord> Steps { get; set; } = [startingAt];

        public void OnRotate(Direction direction)
        {
            onChange();
        }
        
        public void OnMove(Coord position)
        {
            Steps.Add(position);
            onChange();
        }
    }

    public static IPlayerViewFactory Factory => new ViewFactory();
    private class ViewFactory : IPlayerViewFactory
    {
        public IPlayerView Create(Player player, Action onChange) => new PlayerPathView(player, onChange);
    }
}