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

        e.Graphics.FillRectangle(Brushes.White, upperTankFigure);
        e.Graphics.FillRectangle(Brushes.Black, bottomTankFigure);
        
        foreach (Wall wall in _walls)
        {
            PaintWall(e,wall);
        }

        foreach (var bullet in _bullets)
        {
            PaintBullet(e,bullet);
        }
    }

    protected void PaintWall(PaintEventArgs e,Wall wall)
    {
        var wallFigure = new Rectangle(wall.LeftX, wall.UpperY, wall.Width, wall.Height);
        e.Graphics.FillRectangle(Brushes.Chocolate,wallFigure);
    }
    protected void PaintBullet(PaintEventArgs e,Bullet bullet)
    {
        var bulletFigure = new Rectangle(bullet.LeftX, bullet.UpperY, bullet.Width, bullet.Height);
        e.Graphics.FillRectangle(Brushes.DarkRed,bulletFigure);
    }

    public void AnimateBullet()
    {
        lock (_bullets)
        {
            foreach (Bullet bullet in _bullets)
            {
                bullet.Move();
            }
        }
        
    }
}