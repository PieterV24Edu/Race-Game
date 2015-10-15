using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaceGame
{
    public partial class Form1 : Form
    {
        Bitmap Backbuffer;
        Point Blockpoint = new Point(50, 50);
        Point BlockSpeed = new Point(0, 0);

        int holdTimer = 0;
        int currentSpeed = 0;
        int maxSpeed = 10;

        List<Keys> keysPressed = new List<Keys>(4);

        float angle;
        int[] size = new int[2] {40, 20};
        bool Moving = false;

        Image car1 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "carCyan.png"));
        Image car2 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "carDarkGreen.png"));

        Image backImage = new Bitmap(Path.Combine(Environment.CurrentDirectory, "carcircuit.png"));

        public Form1()
        {
            InitializeComponent();

            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer, true);

            Timer GameTimer = new Timer();
            GameTimer.Interval = 10;
            GameTimer.Tick += new EventHandler(GameTimer_Tick);
            GameTimer.Start();

            this.ResizeEnd += new EventHandler(Form1_CreateBackBuffer);
            this.Load += new EventHandler(Form1_CreateBackBuffer);
            this.Paint += new PaintEventHandler(Form1_Paint);
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.KeyUp += new KeyEventHandler(Form1_KeyUp);

            Auto.Image = car1;
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!keysPressed.Contains(e.KeyCode))
            {
                keysPressed.Add(e.KeyCode);
            }
            else
            {
                holdTimer ++;
            }


            foreach (Keys keyPressed in keysPressed)
            {
                switch (keyPressed)
                {
                    case Keys.A:
                        if (Moving == true)
                        {
                            angle += -10f;
                        }

                        break;
                    case Keys.D:
                        if (Moving == true)
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

        void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(keysPressed.Contains(e.KeyCode))
                keysPressed.Remove(e.KeyCode);


            if (!keysPressed.Contains(Keys.S) && !keysPressed.Contains(Keys.W))
            {
                SlowDown();
            }
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
           if (Backbuffer != null)
            {
                e.Graphics.DrawImageUnscaled(Backbuffer, Point.Empty);
            }
        }

        void Form1_CreateBackBuffer(object sender, EventArgs e)
        {
            if(Backbuffer != null)
            {
                Backbuffer.Dispose();
            }
            Backbuffer = new Bitmap(ClientSize.Width, ClientSize.Height);
        }

        void Draw()
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

        void MoveCar()
        {
            Point tempPoint = new Point(Blockpoint.X - size[0] / 2, Blockpoint.Y - size[1] / 2);

            Auto.Location = tempPoint;
        }

        void SlowDown()
        {
            while (currentSpeed > 0)
            {
                currentSpeed --;
                Debug.Print(currentSpeed+"");
            }
        }

        double DegtoRad(double deg)
        {
            return (Math.PI/180)*deg;
        }

        Point CalcMovePoint(float speed, float angle2)
        {
            int py = Convert.ToInt32(speed * (Math.Sin(DegtoRad(angle2))));
            int px = Convert.ToInt32(speed * (Math.Cos(DegtoRad(angle2))));
            
            return new Point(px, py);
        }

        void GameTimer_Tick(object sender, EventArgs e)
        {
            Blockpoint.X += BlockSpeed.X;
            Blockpoint.Y += BlockSpeed.Y;

            BlockSpeed = CalcMovePoint(currentSpeed, angle);
            Draw();
        }

        void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

