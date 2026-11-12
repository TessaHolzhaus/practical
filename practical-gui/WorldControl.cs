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

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                float x = c * cellSize + offsetX;
                float y = r * cellSize + offsetY;

                Brush cellBrush = World.Grid[r, c] switch
                {
                    Field.Wall => Brushes.DarkSlateGray,
                    Field.Empty => Brushes.White,
                    Field.Finish => Brushes.LightGreen,
                    _ => throw new UnreachableException()
                };

                g.FillRectangle(cellBrush, x, y, cellSize, cellSize);
                g.DrawRectangle(Pens.LightGray, x, y, cellSize, cellSize);
            }
        }
    }
}