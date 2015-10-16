using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Race_Game_Rewrite
{
    public partial class Form1 : Form
    {
        private float angle;
        private Bitmap Backbuffer;
        private Image backImage = new Bitmap(Path.Combine(Environment.CurrentDirectory, "carcircuit.png"));
        private Point Car1point = new Point(50, 50);
        private Point Car2point = new Point(50, 50);
        private Point Car1Speed = new Point(0, 0);
        private Point Car2Speed = new Point(0, 0);
        private Image car1 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "carCyan.png"));
        private Image car2 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "carDarkGreen.png"));
        private int currentSpeed;
        private int maxSpeed = 10;
        List<Keys> keysPressed = new List<Keys>(); 

        public Form1()
        {
            InitializeComponent();
        }
    }
}
