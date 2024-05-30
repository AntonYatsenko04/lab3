using System.Runtime.InteropServices;

namespace Lab3Tanks;

using static System.Windows.Forms.Form;

public partial class MainForm
{
    public void GetCollidableConstraints(ICollidable first,
        ICollidable second, out List<Direction> firstConstraints, out List<Direction> secondConstraints)
    {
        firstConstraints = new List<Direction>();
        secondConstraints = new List<Direction>();

        int closeInterval = Constants.CloseInterval;

        bool firstHigherThanSecond = first.UpperY >= second.LowerY;
        bool secondHigherThanFirst = second.UpperY >= first.LowerY;
        bool firstAndSecondHaveCollideVertically = !firstHigherThanSecond && !secondHigherThanFirst;

        bool firstCloseToSecondFromLeft =
            first.RightX >= second.LeftX - closeInterval && first.RightX <= second.LeftX + closeInterval;
        bool firstCloseToSecondFromRight =
            first.LeftX <= second.RightX + closeInterval && first.LeftX >= second.RightX - closeInterval;

        bool firstMoreLeftThanSecond = first.RightX <= second.LeftX;
        bool secondMoreLeftThanFirst = second.RightX <= first.LeftX;
        bool firstAndSecondCollideHorisontally = !firstMoreLeftThanSecond && !secondMoreLeftThanFirst;

        bool firstCloseToSecondFromTop = first.UpperY <= second.LowerY + closeInterval &&
                                         first.UpperY >= second.LowerY - closeInterval;
        bool firstCloseToSecondFromBottom = first.LowerY >= second.UpperY - closeInterval &&
                                            first.LowerY <= second.UpperY + closeInterval;


        if (firstAndSecondHaveCollideVertically)
        {
            if (firstCloseToSecondFromLeft)
            {
                firstConstraints.Add(Direction.Right);
                secondConstraints.Add(Direction.Left);
            }

            if (firstCloseToSecondFromRight)
            {
                firstConstraints.Add(Direction.Left);
                secondConstraints.Add(Direction.Right);
            }
        }


        if (firstAndSecondCollideHorisontally)
        {
            if (firstCloseToSecondFromBottom)
            {
                firstConstraints.Add(Direction.Down);
                secondConstraints.Add(Direction.Up);
            }

            if (firstCloseToSecondFromTop)
            {
                firstConstraints.Add(Direction.Up);
                secondConstraints.Add(Direction.Down);
            }
        }

        firstConstraints.AddRange(_getClientCollisionConstraints(first));
        secondConstraints.AddRange(_getClientCollisionConstraints(second));
    }

    private List<Direction> _getClientCollisionConstraints(ICollidable obj)
    {
        var constraints = new List<Direction>();
        var clientSize = ClientRectangle.Size;
        bool cantMoveRight = obj.RightX >= ClientRectangle.Right;
        bool cantMoveUp = obj.UpperY <= 0;
        bool cantMoveLeft = obj.LeftX <= 0;
        bool cantMoveDown = obj.LowerY >= ClientRectangle.Bottom;

        if (cantMoveRight)
        {
            constraints.Add(Direction.Right);
        }

        if (cantMoveDown)
        {
            constraints.Add(Direction.Down);
        }

        if (cantMoveLeft)
        {
            constraints.Add(Direction.Left);
        }

        if (cantMoveUp)
        {
            constraints.Add(Direction.Up);
        }

        return constraints;
    }

    public void UpdateWallTankConstraints()
    {
        lock (_upperTankConstraints)
        {
            lock (_lowerTankConstraints)
            {
                foreach (Wall wall in _walls)
                {
                    GetCollidableConstraints(_upperTank, wall, out var upperTankConstraints, out _);
                    GetCollidableConstraints(_lowerTank, wall, out var lowerTankConstraints, out _);

                    _upperTankConstraints.AddRange(upperTankConstraints);
                    _lowerTankConstraints.AddRange(lowerTankConstraints);
                }
            }
        }
    }

    public void CheckBulletClientCollision()
    {
        lock (_bullets)
        {
            for (var i = 0; i < _bullets.Count; i++)
            {
                var bullet = _bullets[i];
                var bulletConstraints = _getClientCollisionConstraints(bullet);
                
                if (bulletConstraints.Count > 0)
                {
                    _bullets.Remove(bullet);
                    
                }
            }
        }
    }
    
    public void CheckBulletWallCollision()
    {
        lock (_bullets)
        {
            lock (_walls)
            {
                for (var i = 0; i < _bullets.Count; i++)
                {
                    var bullet = _bullets[i];
                    for (var j = 0; j < _walls.Count; j++)
                    {
                        var wall = _walls[j];
                        GetCollidableConstraints(bullet, wall, out var bulletConstraints, out var _);
                        if (bulletConstraints.Count > 0)
                        {
                            Console.WriteLine($"{i} {j}");
                            _bullets.RemoveAt(i);
                            _walls.RemoveAt(j);
                            break;
                        }
                    }
                }
            }
        }
    }

    public void CheckBulletTankCollision()
    {
        lock (_bullets)
        {
            lock (_upperTank)
            {
                lock (_lowerTank)
                {
                    foreach (Bullet bullet in _bullets)
                    {
                        GetCollidableConstraints(bullet, _upperTank, out var bulletToUpperConstraints, out var _);
                        GetCollidableConstraints(bullet, _lowerTank, out var bulletToLowerConstraints, out var _);

                        if (bulletToUpperConstraints.Count > 0 && bullet.TankNavigator != _upperTank)
                        {
                            MessageBox.Show("lower win");
                            _bullets.Remove(bullet);
                            return;
                        }

                        if (bulletToLowerConstraints.Count > 0 && bullet.TankNavigator != _lowerTank)
                        {
                            MessageBox.Show("up win");
                            _bullets.Remove(bullet);
                            return;
                        }
                    }
                }
            }
        }
    }
}