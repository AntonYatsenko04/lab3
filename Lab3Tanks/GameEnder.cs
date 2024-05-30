namespace Lab3Tanks;

public partial class MainForm
{
    public void UpperWins()
    {
        
        StopThreads();
        ResetGame();
        MessageBox.Show("Победа белых");
    }

    public void LowerWins()
    {
       
        StopThreads();
        ResetGame();
        MessageBox.Show("Победа черных");
    }

    public void ResetGame()
    {
        lock (_lowerTankConstraints)
        {
            _lowerTankConstraints.Clear();
        }

        lock (_upperTankConstraints)
        {
            _upperTankConstraints.Clear();
        }

        InitKeys();

        SetNewTanks();
        _bullets.Clear();
        SetNewWallsAndBase();
        InitThreads();
    }

    public void SetNewWallsAndBase()
    {
        lock (_walls)
        {
            _walls.Clear();
            _walls.Add(new Wall(leftX: 280, upperY: 0, width: Constants.WallWidth, height: Constants.WallHeight));
            _walls.Add(new Wall(leftX: 280, upperY: 50, width: Constants.WallWidth, height: Constants.WallHeight));
            _walls.Add(new Wall(leftX: 330, upperY: 50, width: Constants.WallWidth, height: Constants.WallHeight));
            _walls.Add(new Wall(leftX: 380, upperY: 50, width: Constants.WallWidth, height: Constants.WallHeight));
            _walls.Add(new Wall(leftX: 380, upperY: 0, width: Constants.WallWidth, height: Constants.WallHeight));
            _upperBase = new Wall(leftX: 330, upperY: 0, width: Constants.WallWidth, height: Constants.WallHeight);

            _walls.Add(new Wall(leftX: 280, upperY: 500, width: Constants.WallWidth, height: Constants.WallHeight));
            _walls.Add(new Wall(leftX: 280, upperY: 450, width: Constants.WallWidth, height: Constants.WallHeight));
            _walls.Add(new Wall(leftX: 330, upperY: 450, width: Constants.WallWidth, height: Constants.WallHeight));
            _walls.Add(new Wall(leftX: 380, upperY: 450, width: Constants.WallWidth, height: Constants.WallHeight));
            _walls.Add(new Wall(leftX: 380, upperY: 500, width: Constants.WallWidth, height: Constants.WallHeight));
            _lowerBase = new Wall(leftX: 330, upperY: 500, width: Constants.WallWidth, height: Constants.WallHeight);
        }
    }

    public void InitKeys()
    {
        lock (_pressedKeys)
        {
            _pressedKeys.Clear();
            _pressedKeys.Add(_upperTankUp, false);
            _pressedKeys.Add(_upperTankRight, false);
            _pressedKeys.Add(_upperTankDown, false);
            _pressedKeys.Add(_upperTankLeft, false);
            _pressedKeys.Add(_upperTankShoot, false);

            _pressedKeys.Add(_lowerTankUp, false);
            _pressedKeys.Add(_lowerTankRight, false);
            _pressedKeys.Add(_lowerTankDown, false);
            _pressedKeys.Add(_lowerTankLeft, false);
            _pressedKeys.Add(_lowerTankShoot, false);
        }
    }

    public void InitThreads()
    {
        _upperTankThread = new Thread(() => _driveTank(_upperTank, FieldObject.UpperTank));
        _lowerTankThread = new Thread(() => _driveTank(_lowerTank, FieldObject.LowerTank));
        _commonThread = new Thread(() => _commonLoop());

        _upperTankThread.IsBackground = true;
        _lowerTankThread.IsBackground = true;
           _commonThread.IsBackground = true;
           
           _upperTankThread.Start();
            _lowerTankThread.Start();
           _commonThread.Start();
            
        //         ThreadPool.QueueUserWorkItem(state => _commonLoop());
        // ThreadPool.QueueUserWorkItem(state => _driveTank(_upperTank, FieldObject.UpperTank));
        // ThreadPool.QueueUserWorkItem(state => _driveTank(_lowerTank, FieldObject.LowerTank));
    }

    public void SetNewTanks()
    {
        if (!_isFirstLaunch)
        {
            lock (_upperTank)
            {
                _upperTank = new TankNavigator(upKey: _upperTankUp, rightKey: _upperTankRight, downKey: _upperTankDown,
                    leftKey: _upperTankLeft,
                    _upperTankShoot, 325, 100);
            }

            lock (_lowerTank)
            {
                _lowerTank = new TankNavigator(upKey: _lowerTankUp, rightKey: _lowerTankRight, downKey: _lowerTankDown,
                    leftKey: _lowerTankLeft,
                    _lowerTankShoot, 375, 420);
            }
        }
        else
        {
            _upperTank = new TankNavigator(upKey: _upperTankUp, rightKey: _upperTankRight, downKey: _upperTankDown,
                leftKey: _upperTankLeft,
                _upperTankShoot, 325, 100);

            _lowerTank = new TankNavigator(upKey: _lowerTankUp, rightKey: _lowerTankRight, downKey: _lowerTankDown,
                leftKey: _lowerTankLeft,
                _lowerTankShoot, 375, 420);
        }
    }

    public void StopThreads()
    {
        try
        {
            _commonThread.Abort();
            _lowerTankThread.Abort();
            _upperTankThread.Abort();
        }
        catch (Exception e)
        {
            
        }
        
    }
}