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

        public MainForm()
        {
            this.BackColor = Color.Green;
            this.DoubleBuffered = true;
            MinimumSize = Constants.ClientSize;
            MaximumSize = Constants.ClientSize;

            _upperTank = new TankNavigator(upKey: _upperTankUp, rightKey: _upperTankRight, downKey: _upperTankDown,
                leftKey: _upperTankLeft,
                _upperTankShoot, 10, 10);
            _lowerTank = new TankNavigator(upKey: _lowerTankUp, rightKey: _lowerTankRight, downKey: _lowerTankDown,
                leftKey: _lowerTankLeft,
                _lowerTankShoot, 20, 20);

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

            ThreadPool.QueueUserWorkItem(state => _driveTank(_upperTank, FieldObject.UpperTank));
            ThreadPool.QueueUserWorkItem(state => _driveTank(_lowerTank, FieldObject.LowerTank));
        }

        private void _commonLoop()
        {
        }

        private List<Direction> _getConstraints(FieldObject fieldObject)
        {
            var constraints = new List<Direction>();
            GetCollidableConstraints(_upperTank, _lowerTank, out var upperConstraints,
                out var lowerConstraints
            );
            switch (fieldObject)
            {
                case FieldObject.UpperTank:
                {
                    return upperConstraints;
                }

                    break;
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
                foreach (var keyValuePair in _pressedKeys.Where((e) => e.Value))
                {
                    tankNavigator.Drive(keyValuePair.Key, _getConstraints(fieldObject));
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

                Thread.Sleep(50);
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Console.WriteLine(_upperTank.UpperY);
            Console.WriteLine(_upperTank.RightX);
            var upperTankFigure =
                new Rectangle(_upperTank.LeftX, _upperTank.UpperY, _upperTank.Width, _upperTank.Height);
            var bottomTankFigure =
                new Rectangle(_lowerTank.LeftX, _lowerTank.UpperY, _lowerTank.Width, _lowerTank.Height);

            e.Graphics.FillRectangle(Brushes.Black, upperTankFigure);
            e.Graphics.FillRectangle(Brushes.White, bottomTankFigure);
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