using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace RaceGame
{
    public partial class Form1 : Form
    {
        private float angle;
        private Bitmap Backbuffer;

        private readonly Image backImage = new Bitmap(Path.Combine(Environment.CurrentDirectory, "carcircuit.png"));
        private Point Blockpoint = new Point(50, 50);
        private Point BlockSpeed = new Point(0, 0);

        private readonly Image car1 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "carCyan.png"));
        private Image car2 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "carDarkGreen.png"));
        private int currentSpeed;

        private int holdTimer;

        private readonly List<Keys> keysPressed = new List<Keys>(4);
        private readonly int maxSpeed = 10;
        private bool Moving;
        private readonly int[] size = new int[2] {40, 20};
        private bool Speed;

        public Form1()
        {
            InitializeComponent();

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer, true);

            var GameTimer = new Timer();
            GameTimer.Interval = 10;
            GameTimer.Tick += GameTimer_Tick;
            GameTimer.Start();

            ResizeEnd += Form1_CreateBackBuffer;
            Load += Form1_CreateBackBuffer;
            Paint += Form1_Paint;
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;

            Auto.Image = car1;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!keysPressed.Contains(e.KeyCode))
            {
                keysPressed.Add(e.KeyCode);
            }
            else
            {
                holdTimer ++;
            }


            foreach (var keyPressed in keysPressed)
            {
                switch (keyPressed)
                {
                    case Keys.A:
                        if (Moving)
                        {
                            angle += -10f;
                        }

                        break;
                    case Keys.D:
                        if (Moving)
                        {
                            angle += 10f;
                        }

                        break;
                    case Keys.W:
                        //current speed implement
                        if (currentSpeed < maxSpeed)
                        {
                            currentSpeed ++;
                        }
                        BlockSpeed = CalcMovePoint(currentSpeed, angle);
                        Moving = true;

                        break;
                    case Keys.S:
                        if (currentSpeed < maxSpeed)
                        {
                            currentSpeed++;
                        }
                        BlockSpeed = CalcMovePoint(-currentSpeed, angle);
                        Moving = true;
                        break;
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (keysPressed.Contains(e.KeyCode))
                keysPressed.Remove(e.KeyCode);


            /*if (!keysPressed.Contains(Keys.S) && !keysPressed.Contains(Keys.W))
            {
                SlowDown();
            }*/
            switch (e.KeyCode)
            {
                case Keys.W:
                    SlowDown();
                    break;
                case Keys.S:
                    SlowDown();
                    break;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (Backbuffer != null)
            {
                e.Graphics.DrawImageUnscaled(Backbuffer, Point.Empty);
            }
        }

        private void Form1_CreateBackBuffer(object sender, EventArgs e)
        {
            if (Backbuffer != null)
            {
                Backbuffer.Dispose();
            }
            Backbuffer = new Bitmap(ClientSize.Width, ClientSize.Height);
        }

        private void Draw()
        {
            if (Backbuffer != null)
            {
                using (var g = Graphics.FromImage(Backbuffer))
                {
                    g.Clear(Color.Green);
                    g.DrawImage(backImage, 0, 10, 1000, 700);

                    //tempPoint.Y += CalcMovePoint(currentSpeed, angle).Y;
                    //tempPoint.X += CalcMovePoint(currentSpeed, angle).X;

                    MoveCar();
                    //g.TranslateTransform(Blockpoint.X - size[0]/2.0f, Blockpoint.Y - size[1] / 2.0f);
                    //g.TranslateTransform(-Blockpoint.X - size[0] / 2.0f, -Blockpoint.Y - size[1] / 2.0f);
                    //g.DrawImageUnscaled(, Blockpoint.X, Blockpoint.Y, size[0], size[1]);

                    //g.FillRectangle(Brushes.BlueViolet, Blockpoint.X, Blockpoint.Y, size[0], size[1]);
                }
                Invalidate();
            }
        }

        private void MoveCar()
        {
            var tempPoint = new Point(Blockpoint.X - size[0]/2, Blockpoint.Y - size[1]/2);

            Auto.Location = tempPoint;
        }

        private void SlowDown()
        {
            while (currentSpeed > 0)
            {
                currentSpeed --;
                Debug.Print(currentSpeed+"");
            }
        }

        private double DegtoRad(double deg)
        {
            return (Math.PI/180)*deg;
        }

        private Point CalcMovePoint(float speed, float angle2)
        {
            var py = Convert.ToInt32(speed*(Math.Sin(DegtoRad(angle2))));
            var px = Convert.ToInt32(speed*(Math.Cos(DegtoRad(angle2))));

            return new Point(px, py);
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            Blockpoint.X += BlockSpeed.X;
            Blockpoint.Y += BlockSpeed.Y;

            BlockSpeed = CalcMovePoint(currentSpeed, angle);
            Draw();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}