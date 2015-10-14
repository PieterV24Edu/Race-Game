using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        int currentSpeed = 5;
        int maxSpeed = 10;
        int DefaultBlockspeed = 10;

        List<Keys> keysPressed = new List<Keys>(4);

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
                        BlockSpeed.X = -currentSpeed;

                        if (holdTimer > 0)
                        {
                            currentSpeed++;
                        }
                        break;
                    case Keys.D:
                        BlockSpeed.X = currentSpeed;

                        if (holdTimer > 0)
                        {
                            currentSpeed++;
                        }
                        break;
                    case Keys.W:
                        BlockSpeed.Y = -currentSpeed;

                        if (holdTimer > 0)
                        {
                            currentSpeed++;
                        }
                        break;
                    case Keys.S:
                        BlockSpeed.Y = currentSpeed;

                        if (holdTimer > 0)
                        {
                            currentSpeed++;
                        }
                        break;

                }
            }

        }

        void Form1_KeyUp(object sender, KeyEventArgs e)
        {

                foreach (Keys keyPressed in keysPressed)
                {
                    switch (keyPressed)
                    {
                        case Keys.A:
                            BlockSpeed.Y = 0;
                            break;
                        case Keys.D:
                            BlockSpeed.X = 0;
                            break;
                        case Keys.W:
                            BlockSpeed.Y = 0;
                            break;
                        case Keys.S:
                            BlockSpeed.Y = 0;
                            break;

                    }
                }
            currentSpeed = 5;
            keysPressed.Remove(e.KeyCode);

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
                    g.Clear(Color.White);
                    g.FillRectangle(Brushes.BlueViolet, Blockpoint.X, Blockpoint.Y, 20, 20);
                }
                Invalidate();
            }
        }
        void GameTimer_Tick(object sender, EventArgs e)
        {
            Blockpoint.X += BlockSpeed.X;
            Blockpoint.Y += BlockSpeed.Y;
            Draw();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

