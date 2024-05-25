namespace Lab3Tanks;

public class TankNavigator
{
    private Keys _upKey;
    private Keys _rightKey;
    private Keys _downKey;
    private Keys _leftKey;
    private Keys _shootKey;
    private Tank _tank;
    
    public int TankXPos => _tank.XPos;
    public int TankYPos => _tank.YPos;

    public TankNavigator(Keys upKey, Keys rightKey, Keys downKey, Keys leftKey, Keys shootKey, int x,int y)
    {
        _upKey = upKey;
        _rightKey = rightKey;
        _downKey = downKey;
        _leftKey = leftKey;
        _shootKey = shootKey;
        _tank = new Tank(x,y);
    }

    public void Drive(Keys key, List<Direction> constraints)
    {
        if (key == _upKey&&!constraints.Contains(Direction.Up))
        {
            _tank.Move(Direction.Up);
        }
        if (key == _rightKey&&!constraints.Contains(Direction.Right))
        {
            _tank.Move(Direction.Right);
        }
        if (key == _downKey&&!constraints.Contains(Direction.Down))
        {
            _tank.Move(Direction.Down);
        }
        if (key == _leftKey&&!constraints.Contains(Direction.Left))
        {
            _tank.Move(Direction.Left);
        }
        if (key == _shootKey)
        {
            _tank.TryShoot();
        }
    }
}