namespace Lab3Tanks;

public partial class MainForm
{
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Console.WriteLine("OnPaint");
        Console.WriteLine(DateTime.Now);
        var upperTankFigure =
            new Rectangle(_upperTank.LeftX, _upperTank.UpperY, _upperTank.Width, _upperTank.Height);
        var bottomTankFigure =
            new Rectangle(_lowerTank.LeftX, _lowerTank.UpperY, _lowerTank.Width, _lowerTank.Height);

        e.Graphics.FillRectangle(Brushes.White, upperTankFigure);
        e.Graphics.FillRectangle(Brushes.Black, bottomTankFigure);
        
        DrawGun(_upperTank.LeftX, _upperTank.UpperY, e,_upperTank.Direction);
        DrawGun(_lowerTank.LeftX, _lowerTank.UpperY, e,_lowerTank.Direction);
        
        foreach (Wall wall in _walls)
        {
            PaintWall(e,wall);
        }

        foreach (var bullet in _bullets)
        {
            PaintBullet(e,bullet);
        }
        
        PaintBases(e);
    }

    protected void PaintWall(PaintEventArgs e,Wall wall,  bool isWall = true)
    {
        var wallFigure = new Rectangle(wall.LeftX, wall.UpperY, wall.Width, wall.Height);
        Brush color;
        if (isWall)
        {
            color = Brushes.Chocolate;
        }
        else
        {
            color = Brushes.Blue;
        }
        
        e.Graphics.FillRectangle(color,wallFigure);
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

    public void PaintBases(PaintEventArgs e)
    {
        PaintWall(e,_lowerBase,false);
        PaintWall(e,_upperBase,false);
    }

    public void DrawGun(int leftX, int upperY,PaintEventArgs e,Direction tankDirection)
    {
        int gunLeftX;
        int gunUpperY;
        int gunSize=3;
        int tankWidth = Constants.TankWidth;
        switch (tankDirection)
        {
            case Direction.Up:
                gunLeftX = leftX + tankWidth / 2- gunSize/2;
                gunUpperY = upperY;
                break;
            case Direction.Right:
                gunLeftX = leftX + tankWidth - gunSize;
                gunUpperY = upperY + tankWidth/2;
                break;
            case Direction.Left:
                gunLeftX = leftX;
                gunUpperY = upperY + tankWidth/2;
                break;
            case Direction.Down:
                gunLeftX = leftX + tankWidth/2 - gunSize/2;
                gunUpperY = upperY + tankWidth-gunSize;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(tankDirection), tankDirection, null);
        }

        var rect = new Rectangle(gunLeftX,gunUpperY,gunSize,gunSize);
        e.Graphics.FillRectangle(Brushes.Blue, rect);
    }
}