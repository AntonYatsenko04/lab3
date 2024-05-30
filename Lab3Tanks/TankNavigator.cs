namespace Lab3Tanks;

public class TankNavigator :ICollidable
{
    private Keys _upKey;
    private Keys _rightKey;
    private Keys _downKey;
    private Keys _leftKey;
    private Keys _shootKey;
    private Tank _tank;
    
    public int LeftX => _tank.XPos;
    public int UpperY => _tank.YPos;

    public int Width => Constants.TankWidth;
    public int Height => Constants.TankHeight;

    public int RightX => LeftX + Width;
    public int LowerY => UpperY + Height;

    
    
    public Direction Direction { get; set; }

    public TankNavigator(Keys upKey, Keys rightKey, Keys downKey, Keys leftKey, Keys shootKey, int x,int y)
    {
        _upKey = upKey;
        _rightKey = rightKey;
        _downKey = downKey;
        _leftKey = leftKey;
        _shootKey = shootKey;
        _tank = new Tank(x,y, Constants.TankSpeed);
    }

    public void Drive(Keys key, List<Direction> constraints)
    {
        if (key == _upKey&&!constraints.Contains(Direction.Up))
        {
            Direction = Direction.Up;
            _tank.Move(Direction.Up);
        }
        if (key == _rightKey&&!constraints.Contains(Direction.Right))
        {
            Direction = Direction.Right;
            _tank.Move(Direction.Right);
        }
        if (key == _downKey&&!constraints.Contains(Direction.Down))
        {
            Direction = Direction.Down;
            _tank.Move(Direction.Down);
        }
        if (key == _leftKey&&!constraints.Contains(Direction.Left))
        {
            Direction = Direction.Left;
            _tank.Move(Direction.Left);
        }
        if (key == _shootKey)
        {
            _tank.TryShoot();
        }
    }
    
    public bool Shoot()
    {
        return _tank.TryShoot();
    }
}