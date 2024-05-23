using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Marioooooo
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource cancellationTokenSource;

        //private Thread ballThread;
        //private Thread topPaddleThread;
        //private Thread bottomPaddleThread;
        private bool running = true;
        private int topScore = 0;
        private int bottomScore = 0;
        private Dictionary<Keys, bool> keysPressed = new Dictionary<Keys, bool>();

        private Rectangle topPaddle;
        private Rectangle bottomPaddle;
        private Rectangle ball;
        private int ballSpeedX = 5;
        private int ballSpeedY = 5;
        private int paddleSpeed = 5;
        private int ballAcceleration = 1;

        // Add bullet properties
        private Rectangle topBullet;
        private Rectangle bottomBullet;
        private bool topBulletFired = false;
        private bool bottomBulletFired = false;
        private int bulletSpeed = 5;
        private DateTime topBulletReloadTime;
        private DateTime bottomBulletReloadTime;

        private DateTime topPaddleStuckReloadTime;
        private DateTime bottomPaddleStuckReloadTime;

        private bool topPaddleStuck = false;
        private bool bottomPaddleStuck = false;

        private System.Windows.Forms.Timer topPaddleStuckTimer = new System.Windows.Forms.Timer { Interval = 1 };
        private System.Windows.Forms.Timer bottomPaddleStuckTimer = new System.Windows.Forms.Timer { Interval = 1 };

        public Form1()
        {
            InitializeComponent();

            cancellationTokenSource = new CancellationTokenSource();

            this.BackColor = Color.Green;
            this.Size = new Size(300, 600); 

            topPaddle = new Rectangle(100, 10, 60, 10);
            bottomPaddle = new Rectangle(100, this.ClientSize.Height - 20, 60, 10);
            ball = new Rectangle(this.ClientSize.Width / 2, this.ClientSize.Height / 2, 10, 10);

            keysPressed[Keys.A] = false;
            keysPressed[Keys.D] = false;
            keysPressed[Keys.Left] = false;
            keysPressed[Keys.Right] = false;

            ThreadPool.QueueUserWorkItem(state => GameLoop(cancellationTokenSource.Token));

            ThreadPool.QueueUserWorkItem(state => PaddleMovement( Keys.Left, Keys.Right, cancellationTokenSource.Token));
            ThreadPool.QueueUserWorkItem(state => PaddleMovement(Keys.A, Keys.D, cancellationTokenSource.Token));
            

            topBullet = new Rectangle(0, 0, 5, 10);
            bottomBullet = new Rectangle(0, 0, 5, 10);
            topBulletReloadTime = DateTime.Now;
            bottomBulletReloadTime = DateTime.Now;


            this.DoubleBuffered = true;
        }
        
        private void PaddleMovement(Keys leftKey, Keys rightKey, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                bool leftPressed = false;
                bool rightPressed = false;

                lock (keysPressed)
                {
                    leftPressed = keysPressed[leftKey];
                    rightPressed = keysPressed[rightKey];
                }

                if (leftKey == Keys.Left || leftKey == Keys.Right)
                {
                    if (topPaddleStuck)
                    {
                        Thread.Sleep(10);
                        continue;
                    }

                    if (leftPressed && topPaddle.X > 0)
                    {
                        UpdatePaddlePosition(true, -paddleSpeed);
                    }
                    else if (rightPressed && topPaddle.X < this.ClientSize.Width - topPaddle.Width)
                    {
                        UpdatePaddlePosition(true, paddleSpeed);
                    }
                }
                else if (leftKey == Keys.A || leftKey == Keys.D)
                {
                    if (bottomPaddleStuck)
                    {
                        Thread.Sleep(10);
                        continue;
                    }

                    if (leftPressed && bottomPaddle.X > 0)
                    {
                        UpdatePaddlePosition(false, -paddleSpeed);
                    }
                    else if (rightPressed && bottomPaddle.X < this.ClientSize.Width - bottomPaddle.Width)
                    {
                        UpdatePaddlePosition(false, paddleSpeed);
                    }
                }

                Thread.Sleep(10);
            }
        }

        private void UpdatePaddlePosition(bool isTopPaddle, int deltaX)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    if (isTopPaddle)
                    {
                        topPaddle.X += deltaX;
                    }
                    else
                    {
                        bottomPaddle.X += deltaX;
                    }
                    this.Refresh();
                }));
            }
            else
            {
                if (isTopPaddle)
                {
                    topPaddle.X += deltaX;
                }
                else
                {
                    bottomPaddle.X += deltaX;
                }
                this.Refresh();
            }
        }

        private void GameLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Console.WriteLine("loop");
                MoveBall();
                MoveBullets();
                CheckBulletPaddleCollision();
                Thread.Sleep(10);

            }
        }
        private void MoveBall()
        {
                ball.X += ballSpeedX;
                ball.Y += ballSpeedY;

                if (ball.IntersectsWith(topPaddle) || ball.IntersectsWith(bottomPaddle))
                {
                    ballSpeedY = -ballSpeedY;
                    ballSpeedY += (ballSpeedY > 0 ? ballAcceleration : -ballAcceleration); 
                }

                if (ball.X < 0 || ball.X > this.ClientSize.Width - ball.Width)
                    ballSpeedX = -ballSpeedX;

                if (ball.Y < 0)
                {
                    bottomScore++;
                    ResetBall();
                }
                else if (ball.Y > this.ClientSize.Height - ball.Height)
                {
                    topScore++;
                    ResetBall();
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
        }
        private void ResetBall()
        {
            ball.X = this.ClientSize.Width / 2;
            ball.Y = this.ClientSize.Height / 2;

            ballSpeedX = 2;
            ballSpeedY = 2; 

        }
        private void MoveBullets()
        {
            if (topBulletFired)
            {
                topBullet.Y += bulletSpeed;
                if (topBullet.Y > this.ClientSize.Height)
                    topBulletFired = false; 
            }

            if (bottomBulletFired)
            {
                bottomBullet.Y -= bulletSpeed;
                if (bottomBullet.Y < 0)
                    bottomBulletFired = false; 
            }
        }
        private void CheckBulletPaddleCollision()
        {
            if ((DateTime.Now - bottomPaddleStuckReloadTime).TotalMilliseconds > 1000) bottomPaddleStuck = false;
            if ((DateTime.Now - topPaddleStuckReloadTime).TotalMilliseconds > 1000) topPaddleStuck = false;
            if (topBulletFired && topBullet.IntersectsWith(bottomPaddle))
            {
                bottomPaddle = new Rectangle(100, this.ClientSize.Height - 20, 30, 10);
                bottomPaddleStuckReloadTime = DateTime.Now;
                topBulletFired = false;
                bottomPaddleStuck = true;
                bottomPaddleStuckTimer.Start();

                // Move the top bullet out of the bottom paddle's area
                topBullet.Y = this.ClientSize.Height + 1; // Move it just outside the form's client area
            }

            if (bottomBulletFired && bottomBullet.IntersectsWith(topPaddle))
            {
                topPaddle = new Rectangle(100, 10, 30, 10);

                topPaddleStuckReloadTime = DateTime.Now;
                bottomBulletFired = false;
                topPaddleStuck = true;
                topPaddleStuckTimer.Start();

                // Move the bottom bullet out of the top paddle's area
                bottomBullet.Y = -1; // Move it just outside the form's client area
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.FillRectangle(Brushes.Blue, topPaddle);
            e.Graphics.FillRectangle(Brushes.Red, bottomPaddle);
            e.Graphics.FillRectangle(Brushes.White, ball);

            e.Graphics.DrawString($"Top Score: {topScore}", this.Font, Brushes.White, 10, 30);
            e.Graphics.DrawString($"Bottom Score: {bottomScore}", this.Font, Brushes.White, 10, this.ClientSize.Height - 40);

            if (topBulletFired)
                e.Graphics.FillRectangle(Brushes.Yellow, topBullet);
            if (bottomBulletFired)
                e.Graphics.FillRectangle(Brushes.Yellow, bottomBullet);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (keysPressed.ContainsKey(e.KeyCode))
                keysPressed[e.KeyCode] = true;

            if (e.KeyCode == Keys.Up && !topBulletFired && DateTime.Now >= topBulletReloadTime)
            {
                topBullet.X = topPaddle.X + topPaddle.Width / 2;
                topBullet.Y = topPaddle.Y + topPaddle.Height;
                topBulletFired = true;
                topBulletReloadTime = DateTime.Now.AddSeconds(10);
            }

            if (e.KeyCode == Keys.W && !bottomBulletFired && DateTime.Now >= bottomBulletReloadTime)
            {
                bottomBullet.X = bottomPaddle.X + bottomPaddle.Width / 2;
                bottomBullet.Y = bottomPaddle.Y - bottomBullet.Height;
                bottomBulletFired = true;
                bottomBulletReloadTime = DateTime.Now.AddSeconds(10);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (keysPressed.ContainsKey(e.KeyCode))
                keysPressed[e.KeyCode] = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            running = false;
            cancellationTokenSource.Cancel();

            //ballThread.Join();
            //topPaddleThread.Join();
            //bottomPaddleThread.Join();
        }
    }
}
