using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Configuration;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RaceGame.Properties;

namespace RaceGame
{
    public partial class MainWindow : Form
    {
        public static Rectangle screenSize = new Rectangle(0,0,1011,729);
        public int time = 0;
        double[] Player1Info = new double[4];
        double[] Player2Info = new double[4];
        string player1Name = "Player 1";
        string player2Name = "Player 2";
        public List<Keys> currentInput
        {
            get { return _currentInput; }
            set
            {
                _currentInput = value;
                AddInputs();
            }
        }

        List<Keys> _currentInput = new List<Keys>();

        GraphicsEngine gEngine;
        Timer GameTimer = new Timer();
        Timer InfoTimer = new Timer();

        List<Player> players = new List<Player>();

        Bitmap car1 = new Bitmap(Resources.carCyan);
        Bitmap car2 = new Bitmap(Resources.carDarkGreen);

        Rectangle pitStopPoint = new Rectangle(481, 604, 40, 96);
        Rectangle finishPoint = new Rectangle(536, 66, 33, 95);

        int counter = 0;

        public MainWindow()
        {
            InitializeComponent();

            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer, true);

            GameTimer.Interval = 10;
            GameTimer.Tick += new EventHandler(GameUpdate);
            GameTimer.Start();

            InfoTimer.Interval = 100;
            InfoTimer.Tick += new EventHandler(InfoUpdate);
            InfoTimer.Start();

            this.KeyDown += new KeyEventHandler(SetKeysDown);
            this.KeyUp += new KeyEventHandler(SetKeysUp);

            //inits graphics engine with a graphics handle
            Graphics g = canvas.CreateGraphics();
            gEngine = new GraphicsEngine(g);

            //GraphicsEngine.AddAsset(new Asset(20,car1, new Point(0, 0), 0), RenderType.Player);

            players.Add(new Player("1",player1Name, new Point(1948, 352), 0, car1, new List<Keys>() { Keys.W, Keys.S, Keys.A, Keys.D }));
            players.Add(new Player("2", player2Name, new Point(1948, 484), 0, car2, new List<Keys>() { Keys.Up, Keys.Down, Keys.Left, Keys.Right }));

        }

        private void GameUpdate(object sender, EventArgs e)
        {
            time ++;

            AddInputs();
            gEngine.GraphicsUpdate(null);

            counter = 0;

            foreach (Player p in players)
            {

                float newX = p.GetCarPos().X;
                float newY = p.GetCarPos().Y;
                
                newX = Math.Abs(newX * p.GetScaleX());
                newY = Math.Abs(newY * p.GetScaleY());
                //finish detection
                Finish(counter, new Rectangle(new Point((int)newX, (int)newY), new Size((int)p.GetImageWidth() * (int)p.GetScaleX(), (int)p.GetImageHeight() * (int)p.GetScaleY())));
                //checkpoint detection
                Checkpoint(new Point(715, 189), new Size(196, 33),
                    new Rectangle(new Point((int) newX, (int) newY),
                        new Size((int) p.GetImageWidth()*(int) p.GetScaleX(),
                            (int) p.GetImageHeight()*(int) p.GetScaleY())), counter, 0);
                Checkpoint(new Point(525, 471), new Size(33, 262),
                    new Rectangle(new Point((int)newX, (int)newY),
                        new Size((int)p.GetImageWidth() * (int)p.GetScaleX(),
                            (int)p.GetImageHeight() * (int)p.GetScaleY())), counter, 1);
                Checkpoint(new Point(100, 430), new Size(253, 33),
                    new Rectangle(new Point((int)newX, (int)newY),
                        new Size((int)p.GetImageWidth() * (int)p.GetScaleX(),
                            (int)p.GetImageHeight() * (int)p.GetScaleY())), counter, 2);
                Checkpoint(new Point(426, 246), new Size(189, 33),
                    new Rectangle(new Point((int)newX, (int)newY),
                        new Size((int)p.GetImageWidth() * (int)p.GetScaleX(),
                            (int)p.GetImageHeight() * (int)p.GetScaleY())), counter, 3);
                Checkpoint(new Point(41, 180), new Size(156, 33),
                    new Rectangle(new Point((int)newX, (int)newY),
                        new Size((int)p.GetImageWidth() * (int)p.GetScaleX(),
                            (int)p.GetImageHeight() * (int)p.GetScaleY())), counter, 4);
                //pitstop detection
                Pitstop(counter, new Rectangle(new Point((int)newX, (int)newY), new Size((int)p.GetImageWidth() * (int)p.GetScaleX(), (int)p.GetImageHeight() * (int)p.GetScaleY())));
                counter++;

            }

            Invalidate();
        }

        private void InfoUpdate(object sender, EventArgs e)
        {
            int count = 1;

            foreach (Player p in players)
            {
                if (count == 1)
                {
                    Player1Info = p.GetInfo();
                    count++;
                }
                else if(count == 2)
                {
                    Player2Info = p.GetInfo();
                    count--;
                }
            }

            Meter1.Text = "Player 1\r\nSpeed: " + (int)Player1Info[0]*12 + "\r\nFuel: " + (int)Player1Info[1] + "\r\nLaps: " + (int)Player1Info[2] + "/" + (int)Player1Info[4] + "\r\nPits: " + (int)Player1Info[3];
            Meter2.Text = "Player 2\r\n" + (int)Player2Info[0]*12 + " :Speed\r\n" + (int)Player2Info[1] + " :Fuel\r\n" + (int)Player2Info[2] + "/" + (int)Player1Info[4] + " :Laps\r\n" + (int)Player2Info[3] + " :Pits";
        }

        private void SetKeysUp(object sender, KeyEventArgs e)
        {
            if (currentInput.Contains(e.KeyCode))
            {
                foreach (Player p in players)
                {
                        p.KeyLetGo(e.KeyCode);
                }
                currentInput.Remove(e.KeyCode);
            }
        }

        private void SetKeysDown(object sender, KeyEventArgs e)
        {
            if (!currentInput.Contains(e.KeyCode))
            {
                currentInput.Add(e.KeyCode);
            }
        }

        private void Exit(object sender, FormClosedEventArgs e)
        {
            Invalidate();
        }

        private void AddInputs()
        {
            foreach (Player p in players)
            {
                foreach (Keys k in _currentInput)
                {
                    p.CompareInput(k);
                }
            }
        }
        public bool Finish(int playerNr, Rectangle rectToCompare)
        {

            int width = finishPoint.Width;
            int height = finishPoint.Height;

            Rectangle pitRect = new Rectangle(finishPoint.X, finishPoint.Y, width, height);

            //canvas.CreateGraphics().DrawRectangle(new Pen(Color.Blue), pitRect);

            if (pitRect.Contains(rectToCompare))
            {
                players[playerNr].Finish();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Pitstop(int playerNr,Rectangle rectToCompare)
        {

            int width = pitStopPoint.Width;
            int height = pitStopPoint.Height;

            Rectangle pitRect = new Rectangle(pitStopPoint.X, pitStopPoint.Y, width, height);

            //canvas.CreateGraphics().DrawRectangle(new Pen(Color.Blue), pitRect);

            if (pitRect.Contains(rectToCompare))
            {
                players[playerNr].Refuel();

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Checkpoint(Point PointPos,Size checkpointSize, Rectangle rectToCompare,int playerNr, int checkpoint)
        {

            int width = checkpointSize.Width;
            int height = checkpointSize.Height;

            Rectangle pitRect = new Rectangle(PointPos.X, PointPos.Y, width, height);

            //canvas.CreateGraphics().DrawRectangle(new Pen(Color.Blue), pitRect);
            if (pitRect.Contains(rectToCompare))
            {
                players[playerNr].Checkpoint(checkpoint);
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}

