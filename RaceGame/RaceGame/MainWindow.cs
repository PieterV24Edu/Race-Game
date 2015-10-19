using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Configuration;
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
        public static int width = 640, height= 480;
        public int time = 0;
        public int playerCount = 1;

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

        List<Player> players = new List<Player>();

        Bitmap car1 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "carCyan.png"));
        Bitmap car2 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "carDarkGreen.png"));

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

            this.Paint += new PaintEventHandler(Draw);
            this.KeyDown += new KeyEventHandler(SetKeysDown);
            this.KeyUp += new KeyEventHandler(SetKeysUp);


            GraphicsEngine.AddAsset(new Asset(0,Resources.Background, Point.Empty, 0,new Size(1,1)), RenderType.Background);
            //GraphicsEngine.AddAsset(new Asset(20,car1, new Point(0, 0), 0), RenderType.Player);

            players.Add(new Player("1",new Point(0,0),0,car1,new List<Keys>(){Keys.W,Keys.S,Keys.A,Keys.D}));
            //players.Add(new Player("2", new Point(0,50),0,car2,new List<Keys>(){Keys.Up,Keys.Down,Keys.Left,Keys.Right}));
       }

        private void GameUpdate(object sender, EventArgs e)
        {
            time ++;

            AddInputs();
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

        private void Draw(object sender, PaintEventArgs e)
        {
            Graphics g = canvas.CreateGraphics();
            gEngine = new GraphicsEngine(g);
            gEngine.Start();
        }

        private void Exit(object sender, FormClosingEventArgs e)
        {
            gEngine.Stop();
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

    }

}

