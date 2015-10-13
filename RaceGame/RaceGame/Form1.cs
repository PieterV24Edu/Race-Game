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
        int DefaultBlockspeed = 10;
        bool W = false, A = false, S = false, D = false;

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
            if (e.KeyCode == Keys.A)
            {
                BlockSpeed.X = -DefaultBlockspeed;
                A = true;
            }
            else if (e.KeyCode == Keys.D)
            {
                BlockSpeed.X = DefaultBlockspeed;
                D = true;
            }
            else if (e.KeyCode == Keys.W)
            {
                BlockSpeed.Y = -DefaultBlockspeed;
                W = true;
            }
            else if (e.KeyCode == Keys.S)
            {
                BlockSpeed.Y = DefaultBlockspeed;
                S = true;
            }
        }
        void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.A)
            {
                if(A == true)
                {
                    BlockSpeed.X += DefaultBlockspeed;
                    A = false;
                }
            }
            else if (e.KeyCode != Keys.D)
            {
                if (D == true)
                {
                    BlockSpeed.X += -DefaultBlockspeed;
                    D = false;
                }
            }
            else if (e.KeyCode != Keys.W)
            {
                if (W == true)
                {
                    BlockSpeed.Y += DefaultBlockspeed;
                    W = false;
                }
            }
            else if (e.KeyCode != Keys.S)
            {
                if (S == true)
                {
                    BlockSpeed.Y += -DefaultBlockspeed;
                    S = false;
                }
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
    }
}

