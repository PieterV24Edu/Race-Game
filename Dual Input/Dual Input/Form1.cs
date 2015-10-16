using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dual_Input
{
    public partial class Form1 : Form
    {
        List<Keys> Player1Keys = new List<Keys>();
        List<Keys> Player2Keys = new List<Keys>(); 
        Keys[] Player1Entries = new Keys[4] {Keys.W, Keys.A, Keys.S, Keys.D};
        Keys[] Player2Entries = new Keys[4] { Keys.Up, Keys.Left, Keys.Down, Keys.Right };
        public Form1()
        {
            InitializeComponent();
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;

            var ThisTimer = new Timer();
            ThisTimer.Interval = 10;
            ThisTimer.Tick += Timer_Tick;
            ThisTimer.Start();
        }

        public void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (Keys key in Player1Entries)
            {
                if (!Player1Keys.Contains(e.KeyCode))
                {
                    if (e.KeyCode == key)
                    {
                        Player1Keys.Add(e.KeyCode);
                    }
                }
            }
            foreach (Keys key in Player2Entries)
            {
                if (!Player2Keys.Contains(e.KeyCode))
                {
                    if (e.KeyCode == key)
                    {
                        Player2Keys.Add(e.KeyCode);
                    }
                }
            }
        }

        public void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (Player1Keys.Contains(e.KeyCode))
            {
                Player1Keys.Remove(e.KeyCode);
            }
            if (Player2Keys.Contains(e.KeyCode))
            {
                Player2Keys.Remove(e.KeyCode);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int[,] ActiveKeys = new int[2, 4];
            for (int i = 0; i < 2; i++)
            {
                for (int b = 0; b < 4; b++)
                {
                    ActiveKeys[i, b] = 0;
                }
            }
            foreach (Keys key in Player1Keys)
            {
                switch (key)
                {
                    case Keys.W:
                        ActiveKeys[0, 0] = 1;
                        break;
                    case Keys.A:
                        ActiveKeys[0, 1] = 1;
                        break;
                    case Keys.S:
                        ActiveKeys[0, 2] = 1;
                        break;
                    case Keys.D:
                        ActiveKeys[0, 3] = 1;
                        break;
                }
            }
            foreach (Keys key in Player2Keys)
            {
                switch (key)
                {
                    case Keys.Up:
                        ActiveKeys[1, 0] = 1;
                        break;
                    case Keys.Left:
                        ActiveKeys[1, 1] = 1;
                        break;
                    case Keys.Down:
                        ActiveKeys[1, 2] = 1;
                        break;
                    case Keys.Right:
                        ActiveKeys[1, 3] = 1;
                        break;
                }
            }
            for (int i = 0; i < 2; i++)
            {
                Debug.Print("Player " + i);
                for (int b = 0; b < 4; b++)
                {
                    Debug.Print(ActiveKeys[i, b] + "");
                }
            }
        }
    }
}
