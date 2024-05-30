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
        private Keys _lowerTankShoot = Keys.Enter;

        private Dictionary<Keys, bool> _pressedKeys = new();

        private TankNavigator _upperTank;
        private TankNavigator _lowerTank;

        private List<Direction> _upperTankConstraints = new();
        private List<Direction> _lowerTankConstraints = new();

        private List<Bullet> _bullets = new List<Bullet>();
        private List<Wall> _walls = new List<Wall>();

        private Wall _upperBase;
        private Wall _lowerBase;

        private bool _isFirstLaunch = true;

        private Thread _upperTankThread;
        private Thread _lowerTankThread;
        private Thread _commonThread;

        public MainForm()
        {
            this.BackColor = Color.Green;
            this.DoubleBuffered = true;
            MinimumSize = Constants.ClientSize;
            MaximumSize = Constants.ClientSize;

           SetNewTanks();
            InitKeys();
            
            SetNewWallsAndBase();
            
            InitThreads();
        }

        private void _commonLoop()
        {
            while (true)
            {
                UpdateWallTankConstraints();
                ShootKey();
                AnimateBullet();
                CheckBulletClientCollision();
                CheckBulletWallCollision();
                CheckBulletBaseCollision();
                CheckBulletTankCollision();
                Thread.Sleep(Constants.SleepTimeout);
            }
        }

        private List<Direction> _getConstraints(FieldObject fieldObject)
        {
            var allUpperConstraints= new List<Direction>();
            var allLowerConstraints = new List<Direction>();
            lock (_walls)
            {
                foreach (Wall wall in _walls)
                {
                    GetCollidableConstraints(_upperTank, wall, out var upperTankConstraints, out _);
                    GetCollidableConstraints(_lowerTank, wall, out var lowerTankConstraints, out _);

                    allUpperConstraints.AddRange(upperTankConstraints);
                    allLowerConstraints.AddRange(lowerTankConstraints);
                }
      
                GetCollidableConstraints(_upperTank, _lowerTank, out var upperConstraints,
                    out var lowerConstraints
                );
                
                GetCollidableConstraints(_upperTank,_upperBase,out var upperTankUpperBaseConstraints,out var _);
                GetCollidableConstraints(_upperTank,_lowerBase,out var upperTankLowerBaseConstraints,out var _);
                GetCollidableConstraints(_lowerTank,_upperBase,out var lowerTankUpperBaseConstraints,out var _);
                GetCollidableConstraints(_lowerTank,_lowerBase,out var lowerTankLowerBaseConstraints,out var _);
            
                upperConstraints.AddRange(allUpperConstraints);
                upperConstraints.AddRange(upperTankUpperBaseConstraints);
                upperConstraints.AddRange(upperTankLowerBaseConstraints);
                
                lowerConstraints.AddRange(allLowerConstraints);
                lowerConstraints.AddRange(lowerTankUpperBaseConstraints);
                lowerConstraints.AddRange(lowerTankLowerBaseConstraints);
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
            {
                
                _pressedKeys[e.KeyCode] = true;
                Console.WriteLine(e.KeyData);
            }
                
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (_pressedKeys.ContainsKey(e.KeyCode))
                _pressedKeys[e.KeyCode] = false;
        }
    }
}