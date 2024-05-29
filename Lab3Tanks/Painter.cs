namespace Lab3Tanks;

public partial class MainForm
{
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        
        var upperTankFigure =
            new Rectangle(_upperTank.LeftX, _upperTank.UpperY, _upperTank.Width, _upperTank.Height);
        var bottomTankFigure =
            new Rectangle(_lowerTank.LeftX, _lowerTank.UpperY, _lowerTank.Width, _lowerTank.Height);

        e.Graphics.FillRectangle(Brushes.Black, upperTankFigure);
        e.Graphics.FillRectangle(Brushes.White, bottomTankFigure);
        
        foreach (Wall wall in _walls)
        {
            PaintWall(e,wall);
        }
    }

    protected void PaintWall(PaintEventArgs e,Wall wall)
    {
        var wallFigure = new Rectangle(wall.LeftX, wall.UpperY, wall.Width, wall.Height);
        e.Graphics.FillRectangle(Brushes.Chocolate,wallFigure);
    }
}