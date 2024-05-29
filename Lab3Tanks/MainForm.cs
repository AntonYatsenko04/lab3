using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Lab3Tanks;

namespace Lab3Tanks
{
    public partial class MainForm : Form
    {
        private Keys _upperTankUp = Keys.W;
        private Keys _upperTankRight = Keys.D;
        private Keys _upperTankDown = Keys.S;
        private Keys _upperTankLeft = Keys.A;
        private Keys _upperTankShoot = Keys.Space;

        private Keys _lowerTankUp = Keys.Up;
        private Keys _lowerTankRight = Keys.Right;
        private Keys _lowerTankDown = Keys.Down;
        private Keys _lowerTankLeft = Keys.Left;
        private Keys _lowerTankShoot = Keys.RShiftKey;

        private Dictionary<Keys, bool> _pressedKeys = new();

        private TankNavigator _upperTank;
        private TankNavigator _lowerTank;

        private List<Direction> _upperTankConstraints = new();
        private List<Direction> _lowerTankConstraints = new();

        private List<Wall> _walls = new List<Wall>();

        public MainForm()
        {
            this.BackColor = Color.Green;
            this.DoubleBuffered = true;
            MinimumSize = Constants.ClientSize;
            MaximumSize = Constants.ClientSize;

            _upperTank = new TankNavigator(upKey: _upperTankUp, rightKey: _upperTankRight, downKey: _upperTankDown,
                leftKey: _upperTankLeft,
                _upperTankShoot, 250, 10);
            _lowerTank = new TankNavigator(upKey: _lowerTankUp, rightKey: _lowerTankRight, downKey: _lowerTankDown,
                leftKey: _lowerTankLeft,
                _lowerTankShoot, 100, 100);

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
            
            _walls.Add(new Wall(leftX:280,upperY:0,width:Constants.WallWidth,height:Constants.WallHeight));
            _walls.Add(new Wall(leftX:280,upperY:50,width:Constants.WallWidth,height:Constants.WallHeight));
            _walls.Add(new Wall(leftX:330,upperY:50,width:Constants.WallWidth,height:Constants.WallHeight));
            _walls.Add(new Wall(leftX:380,upperY:50,width:Constants.WallWidth,height:Constants.WallHeight));
            _walls.Add(new Wall(leftX:380,upperY:0,width:Constants.WallWidth,height:Constants.WallHeight));
            _walls.Add(new Wall(leftX:280,upperY:500,width:Constants.WallWidth,height:Constants.WallHeight));
            _walls.Add(new Wall(leftX:280,upperY:450,width:Constants.WallWidth,height:Constants.WallHeight));
            _walls.Add(new Wall(leftX:330,upperY:450,width:Constants.WallWidth,height:Constants.WallHeight));
            _walls.Add(new Wall(leftX:380,upperY:450,width:Constants.WallWidth,height:Constants.WallHeight));
            _walls.Add(new Wall(leftX:380,upperY:500,width:Constants.WallWidth,height:Constants.WallHeight));

            ThreadPool.QueueUserWorkItem(state => _commonLoop());
            ThreadPool.QueueUserWorkItem(state => _driveTank(_upperTank, FieldObject.UpperTank));
            ThreadPool.QueueUserWorkItem(state => _driveTank(_lowerTank, FieldObject.LowerTank));
        }

        private void _commonLoop()
        {
            while (true)
            {

                lock (_upperTankConstraints)
                {
                    lock (_lowerTankConstraints)
                    {
                        foreach (Wall wall in _walls)
                        {
                            GetCollidableConstraints(_upperTank, wall, out var upperTankConstraints, out _);
                            Console.WriteLine("new wall con");
                            upperTankConstraints.ForEach((e)=>Console.WriteLine(e));
                            Console.WriteLine("itog");
                            _upperTankConstraints.ForEach((e)=>Console.WriteLine(e));
                            
                            
                            GetCollidableConstraints(_lowerTank, wall, out var lowerTankConstraints, out _);

                            _upperTankConstraints.AddRange(upperTankConstraints);
                            _lowerTankConstraints.AddRange(lowerTankConstraints);
                        }
                    }

                    
                }
                Thread.Sleep(Constants.SleepTimeout);
            }
        }

        private List<Direction> _getConstraints(FieldObject fieldObject)
        {
      
            GetCollidableConstraints(_upperTank, _lowerTank, out var upperConstraints,
                out var lowerConstraints
            );
            switch (fieldObject)
            {
                case FieldObject.UpperTank:
                {
                    return upperConstraints;
                }

                 
                case FieldObject.LowerTank:
                {
                    return lowerConstraints;
                }

                default:
                    throw new ArgumentOutOfRangeException(nameof(fieldObject), fieldObject, null);
            }

        }


        private void _driveTank(TankNavigator tankNavigator, FieldObject fieldObject)
        {
            while (true)
            {
                lock (_upperTankConstraints)
                {
                    lock (_lowerTankConstraints)
                    {
                        foreach (var keyValuePair in _pressedKeys.Where((e) => e.Value))
                        {
                            if (fieldObject == FieldObject.UpperTank)
                            {
                                _upperTankConstraints.ForEach((e)=>Console.WriteLine(e));
                                Console.WriteLine("sdas");
                                _upperTankConstraints.AddRange(_getConstraints(fieldObject));
                                tankNavigator.Drive(keyValuePair.Key, _upperTankConstraints);
                                _upperTankConstraints.Clear();
                            }
                            if (fieldObject == FieldObject.LowerTank)
                            {
                                _lowerTankConstraints.AddRange(_getConstraints(fieldObject));
                                tankNavigator.Drive(keyValuePair.Key, _lowerTankConstraints);
                                _lowerTankConstraints.Clear();
                            }
                        }
                    }
                    
                }
                

                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    try
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            if (this.IsHandleCreated && !this.IsDisposed)
                            {
                                this.Refresh();
                            }
                        });
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                }

                Thread.Sleep(Constants.SleepTimeout);
            }
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (_pressedKeys.ContainsKey(e.KeyCode))
                _pressedKeys[e.KeyCode] = true;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (_pressedKeys.ContainsKey(e.KeyCode))
                _pressedKeys[e.KeyCode] = false;
        }
    }
}