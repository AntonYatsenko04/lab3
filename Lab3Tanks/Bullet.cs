namespace Lab3Tanks;

public class Bullet:ICollidable
{
    public Bullet(int leftX, int upperY, Direction direction, TankNavigator tankNavigator)
    {
        LeftX = leftX;
        UpperY = upperY;
        Width = Constants.BulletWidth;
        Height = Constants.BulletHeight;
        Direction = direction;
        TankNavigator = tankNavigator;
    }

    public int LeftX { get; private set; }
    public int UpperY { get; private set;}
    public int Width { get; }
    public int Height { get; }
    public int RightX => LeftX + Width;
    public int LowerY => UpperY + Height;

    public Direction Direction;

    public TankNavigator TankNavigator;

    public void Move()
    {
        switch (Direction)
        {
            case Direction.Up:
                UpperY -= Constants.BulletSpeed; 
                break;
            case Direction.Right:
                LeftX += Constants.BulletSpeed; 
                break;
            case Direction.Left:
                LeftX -= Constants.BulletSpeed; 
                break;
            case Direction.Down:
                UpperY += Constants.BulletSpeed; 
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}