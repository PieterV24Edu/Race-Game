using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Race_Game_Rewrite
{
    public partial class Form1 : Form
    {
        //global variables
        List<Keys> Player1Keys = new List<Keys>(4);
        List<Keys> Player2Keys = new List<Keys>(4);
        //pre defined keys for both players
        Keys[] Player1Entries = new Keys[4] { Keys.W, Keys.A, Keys.S, Keys.D };
        Keys[] Player2Entries = new Keys[4] { Keys.Up, Keys.Left, Keys.Down, Keys.Right };
        private int maxSpeed = 10;
        private float accStep = 0.25F;
        private float[] currentSpeed = new float[2] {0 , 0};
        private float[] angle = new float[2] {0, 0};

        public Form1()
        {
            InitializeComponent();
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            //gametimer
            var ThisTimer = new Timer();
            ThisTimer.Interval = 10;
            ThisTimer.Tick += Timer_Tick;
            ThisTimer.Start();
        }

        public void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //add the entered key into the list if it's not yet there and it matches one of the predefined keys for set player.
            foreach (Keys key in Player1Entries)
            {
                if (!Player1Keys.Contains(e.KeyCode))
                {
                    if (e.KeyCode == key)
                    {
                        Player1Keys.Add(e.KeyCode);
                        //Debug.Print(e.KeyCode + " added");
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
                        //Debug.Print(e.KeyCode + " added");
                    }
                }
            }
        }

        public void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //remove key from list if it is present
            if (Player1Keys.Contains(e.KeyCode))
            {
                Player1Keys.Remove(e.KeyCode);
                //Debug.Print(e.KeyCode + " Removed");
            }
            if (Player2Keys.Contains(e.KeyCode))
            {
                Player2Keys.Remove(e.KeyCode);
                //Debug.Print(e.KeyCode + " Removed");
            }
        }
        //acceleration and decelleration
        private void Accelerate(int i, char dir)
        {
            switch (dir)
            {
                case 'F':
                    if (currentSpeed[i] < maxSpeed)
                    {
                        currentSpeed[i] += accStep;
                    }
                    break;
                case 'B':
                    if (currentSpeed[i] > -maxSpeed)
                    {
                        currentSpeed[i] -= accStep;
                    }
                    break;
            }
        }

        private void Decellerate(int i)
        {
            if (currentSpeed[i] > 0)
            {
                currentSpeed[i] -= accStep;
            }
            if (currentSpeed[i] < 0)
            {
                currentSpeed[i] += accStep;
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            //start player 1 controls
            foreach (Keys key in Player1Keys)
            {
                switch (key)
                {
                    case Keys.W:
                            Accelerate(0, 'F');
                        break;
                    case Keys.A:
                        if (currentSpeed[0] != 0) 
                        {
                            angle[0] += 1.0f;
                        }
                        break;
                    case Keys.S:
                        Accelerate(0, 'B');
                        break;
                    case Keys.D:
                        if (currentSpeed[0] != 0)
                        {
                            angle[0] -= 1.0f;
                        }
                        break;
                }
            }
            if (!Player1Keys.Contains(Keys.W) && !Player1Keys.Contains(Keys.S))
            {
                if (currentSpeed[0] != 0)
                {
                    Decellerate(0);
                }
            }
            //end player 1 controls
            //start player 2 controls
            foreach (Keys key in Player2Keys)
            {
                switch (key)
                {
                    case Keys.Up:
                        Accelerate(1, 'F');
                        break;
                    case Keys.Left:
                        if (currentSpeed[1] != 0)
                        {
                            angle[1] += 1.0f;
                        }
                        break;
                    case Keys.Down:
                        Accelerate(1, 'B');
                        break;
                    case Keys.Right:
                        if (currentSpeed[1] != 0)
                        {
                            angle[1] -= 1.0f;
                        }
                        break;
                }
            }
            if (!Player2Keys.Contains(Keys.Up) && !Player2Keys.Contains(Keys.Down))
            {
                if (currentSpeed[1] != 0)
                {
                    Decellerate(1);
                }
            }
            //end player 2 controls
            Debug.Print("Player 1 speed: {0}", currentSpeed[0]);
            Debug.Print("Player 1 angle: {0}", angle[0]);
            Debug.Print("Player 2 speed: {0}", currentSpeed[1]);
            Debug.Print("Player 2 angle: {0}", angle[1]);
        }
    }
}