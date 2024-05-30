namespace Lab3Tanks;

public class Tank
{
    private int _yPos;
    private int _speed;
    private Direction _direction;
    private DateTime _timeOfLastShot;
    private TimeSpan _reloadTime;
    
    public int XPos { get; private set; }

    public int YPos { get; private set; }

    public Tank(int xPos, int yPos, int speed)
    {
        XPos = xPos;
        YPos = yPos;
        _speed = speed;
        _direction = Direction.Up;
        _timeOfLastShot=DateTime.Now;
        _reloadTime = TimeSpan.FromSeconds(1);
    }

    public void Move(Direction direction)
    {
        _direction = direction;
        switch (direction)
        {
            case Direction.Up:
                YPos -= _speed;
                break;
            case Direction.Right:
                XPos += _speed;
                break;
            case Direction.Down:
                YPos += _speed;
                break;
            case Direction.Left:
                XPos -= _speed;
                break;
        }
        
    }

    public bool TryShoot()
    {
        if (DateTime.Now - _timeOfLastShot >= _reloadTime)
        {
            Console.WriteLine("shoot");
            _timeOfLastShot = DateTime.Now;
            return true;
        }

        return false;
    }
}