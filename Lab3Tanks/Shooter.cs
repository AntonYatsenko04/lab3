namespace Lab3Tanks;

public partial class MainForm
{
    public void Shoot(FieldObject fieldObject)
    {
        lock (_bullets)
        {
            Bullet bullet;
            switch (fieldObject)
            {
                case FieldObject.UpperTank:
                    if (_upperTank.Shoot())
                    {
                        Console.WriteLine("ushoot");
                        bullet = new Bullet(leftX: _upperTank.LeftX + Constants.TankWidth / 2,
                            upperY: _upperTank.UpperY + Constants.BulletHeight,_upperTank.Direction, _upperTank);
                        _bullets.Add(bullet);
                    }
                
                
                    break;
                case FieldObject.LowerTank:
                    if (_lowerTank.Shoot())
                    {
                        bullet = new Bullet(leftX: _lowerTank.LeftX + Constants.TankWidth / 2,
                            upperY: _lowerTank.UpperY + Constants.BulletHeight, _lowerTank.Direction, _lowerTank);
                        _bullets.Add(bullet);
                    }
                
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fieldObject), fieldObject, null);
            }

        }
        
    }

    public void ShootKey()
    {
        foreach (var keyValuePair in _pressedKeys.Where((e) => e.Value))
        {
            if (keyValuePair.Key == _upperTankShoot)
            {
                Shoot(FieldObject.UpperTank);
            }
            if (keyValuePair.Key == _lowerTankShoot)
            {
                Shoot(FieldObject.LowerTank);
            }
        }
        
    }
}