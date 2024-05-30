namespace Lab3Tanks;

public partial class MainForm
{
    public void UpperWins()
    {
        MessageBox.Show("Победа белых");
        ResetGame();
    }

    public void LowerWins()
    {
        MessageBox.Show("Победа черных");
        ResetGame();
    }

    public void ResetGame()
    {
    InitKeys();

    _upperTankConstraints.Clear();
    _lowerTankConstraints.Clear();
    
    SetNewTanks();
    
    SetNewWallsAndBase();
    InitThreads();
    }

    public void SetNewWallsAndBase()
    {
        lock (_walls)
        {
            _walls.Clear();
            _walls.Add(new Wall(leftX:280,upperY:0,width:Constants.WallWidth,height:Constants.WallHeight));
            _walls.Add(new Wall(leftX:280,upperY:50,width:Constants.WallWidth,height:Constants.WallHeight));
            _walls.Add(new Wall(leftX:330,upperY:50,width:Constants.WallWidth,height:Constants.WallHeight));
            _walls.Add(new Wall(leftX:380,upperY:50,width:Constants.WallWidth,height:Constants.WallHeight));
            _walls.Add(new Wall(leftX:380,upperY:0,width:Constants.WallWidth,height:Constants.WallHeight));
            _upperBase = new Wall(leftX: 330, upperY: 0, width: Constants.WallWidth, height: Constants.WallHeight);
            
            _walls.Add(new Wall(leftX:280,upperY:500,width:Constants.WallWidth,height:Constants.WallHeight));
            _walls.Add(new Wall(leftX:280,upperY:450,width:Constants.WallWidth,height:Constants.WallHeight));
            _walls.Add(new Wall(leftX:330,upperY:450,width:Constants.WallWidth,height:Constants.WallHeight));
            _walls.Add(new Wall(leftX:380,upperY:450,width:Constants.WallWidth,height:Constants.WallHeight));
            _walls.Add(new Wall(leftX:380,upperY:500,width:Constants.WallWidth,height:Constants.WallHeight));
            _lowerBase = new Wall(leftX:330,upperY:500,width:Constants.WallWidth,height:Constants.WallHeight);
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
        
        ThreadPool.QueueUserWorkItem(state => _commonLoop());
        ThreadPool.QueueUserWorkItem(state => _driveTank(_upperTank, FieldObject.UpperTank));
        ThreadPool.QueueUserWorkItem(state => _driveTank(_lowerTank, FieldObject.LowerTank));
    }

    public void SetNewTanks()
    {
       
            _upperTank = new TankNavigator(upKey: _upperTankUp, rightKey: _upperTankRight, downKey: _upperTankDown,
                leftKey: _upperTankLeft,
                _upperTankShoot, 325, 100);
        

       
            _lowerTank = new TankNavigator(upKey: _lowerTankUp, rightKey: _lowerTankRight, downKey: _lowerTankDown,
                leftKey: _lowerTankLeft,
                _lowerTankShoot, 375, 420);
        
        
    }
}