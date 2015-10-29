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

    /// <summary>
    /// Main class used by the window
    /// </summary>
    public partial class MainWindow : Form
    {
        /// <summary>
        /// The Rectangle that holds the size of the window
        /// </summary>
        public static Rectangle screenSize = new Rectangle(0,0,1011,729);
        
        /// <summary>
        /// Double array for the first player that holds in order: currentSpeed, fuelRemaining, roundsElapsed, timesRefueled
        /// </summary>
        double[] Player1Info = new double[4];
        /// <summary>
        /// Double array for the second player that holds in order: currentSpeed, fuelRemaining, roundsElapsed, timesRefueled
        /// </summary>
        double[] Player2Info = new double[4];
        
        /// <summary>
        /// Name of the first player
        /// </summary>
        public string player1Name;
        
        /// <summary>
        /// Name of the second player
        /// </summary>
        public string player2Name;

        /// <summary>
        /// Amount of rounds to race
        /// </summary>
        public static int requiredRounds;

        /// <summary>
        /// Custom setter for _currentInput to make sure everytime a new input is added the players are updated
        /// </summary>
        public List<Keys> currentInput
        {
            get { return _currentInput; }
            set
            {
                _currentInput = value;
                SendInputs();
            }
        }

        /// <summary>
        /// Holds all buttons currently pressed
        /// </summary>
        List<Keys> _currentInput = new List<Keys>();

        /// <summary>
        /// Instance of the GraphicsEngine used to update the screen
        /// </summary>
        GraphicsEngine gEngine;

        /// <summary>
        /// Timer used as the main game loop
        /// </summary>
        Timer GameTimer = new Timer();

        /// <summary>
        /// Timer used to displaying and updating player information on screen
        /// </summary>
        Timer InfoTimer = new Timer();

        /// <summary>
        /// Holds all the players in the game
        /// </summary>
        List<Player> players = new List<Player>();

        /// <summary>
        /// Holds the image for the car of the first player
        /// </summary>
        Bitmap car1 = new Bitmap(Resources.carCyan);

        /// <summary>
        /// Holds the image for the car of the second player
        /// </summary>
        Bitmap car2 = new Bitmap(Resources.carDarkGreen);

        /// <summary>
        /// Rectangle holding the point on the screen of the pitstop
        /// </summary>
        Rectangle pitStopPoint = new Rectangle(481, 604, 40, 96);

        /// <summary>
        /// Rectangle holding the point on the screen of the finish
        /// </summary>
        Rectangle finishPoint = new Rectangle(536, 66, 33, 95);

        /// <summary>
        /// Player counter used when doing player movement and multiple types of detection
        /// </summary>
        int counter = 0;

        /// <summary>
        /// Main window constructor used to initialize with playernames and a amount of rounds
        /// </summary>
        /// <param name="p1Name">Name of the first player</param>
        /// <param name="p2Name">Name of the second player</param>
        /// <param name="reqRounds">Amount of rounds to race</param>
        public MainWindow(string p1Name, string p2Name, int reqRounds)
        {
            //Initializes the window
            InitializeComponent();

            //Sets the playername input in the main menu
            player1Name = p1Name;
            player2Name = p2Name;
            
            //Amount of rounds to race
            requiredRounds = reqRounds;

            //Set the window to use double buffering and other settings
            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer, true);

            //Set timer parameters for the main game loop
            GameTimer.Interval = 10;
            GameTimer.Tick += new EventHandler(GameUpdate);
            GameTimer.Start();

            //Set timer parameters for the player info window
            InfoTimer.Interval = 100;
            InfoTimer.Tick += new EventHandler(InfoUpdate);
            InfoTimer.Start();

            //Set the key up and key down callbacks
            this.KeyDown += new KeyEventHandler(SetKeysDown);
            this.KeyUp += new KeyEventHandler(SetKeysUp);

            //Initializes graphics engine with a graphics handle
            Graphics g = canvas.CreateGraphics();
            gEngine = new GraphicsEngine(g);

            //Create the players
            players.Add(new Player("1",player1Name, new Point(1948, 352), 0, car1, new List<Keys>() { Keys.W, Keys.S, Keys.A, Keys.D }));
            players.Add(new Player("2", player2Name, new Point(1948, 484), 0, car2, new List<Keys>() { Keys.Up, Keys.Down, Keys.Left, Keys.Right }));

        }

        /// <summary>
        /// Main game loop of the race game
        /// </summary>
        /// <param name="sender">Is not used and will stay null, only in place because a Timer requires it</param>
        /// <param name="e">Is not used and will stay null, only in place because a Timer requires it</param>
        private void GameUpdate(object sender, EventArgs e)
        {
            
            //Gets all inputs detected and compares them to all players
            SendInputs();

            //Redraw to the canvas
            gEngine.GraphicsUpdate();

            //The player number counter
            counter = 0;

            //Loop over all players and change the scale of the car and detect the checkpoints, the finish and the pitstop
            foreach (Player p in players)
            {

                //Get the current car position
                float newX = p.GetCarPos().X;
                float newY = p.GetCarPos().Y;
                
                //Scaling the current car position to it's scale on screen
                newX = Math.Abs(newX * p.GetScaleX());
                newY = Math.Abs(newY * p.GetScaleY());

                //Finish detection
                Finish(counter, new Rectangle(new Point((int)newX, (int)newY), new Size((int)p.GetImageWidth() * (int)p.GetScaleX(), (int)p.GetImageHeight() * (int)p.GetScaleY())));
                
                //Checkpoint1 detection
                Checkpoint(new Point(715, 189), new Size(196, 33),
                    new Rectangle(new Point((int) newX, (int) newY),
                        new Size((int) p.GetImageWidth()*(int) p.GetScaleX(),
                            (int) p.GetImageHeight()*(int) p.GetScaleY())), counter, 0);
                //Checkpoint2 detection
                Checkpoint(new Point(525, 471), new Size(33, 262),
                    new Rectangle(new Point((int)newX, (int)newY),
                        new Size((int)p.GetImageWidth() * (int)p.GetScaleX(),
                            (int)p.GetImageHeight() * (int)p.GetScaleY())), counter, 1);
                //Checkpoint3 detection
                Checkpoint(new Point(100, 430), new Size(253, 33),
                    new Rectangle(new Point((int)newX, (int)newY),
                        new Size((int)p.GetImageWidth() * (int)p.GetScaleX(),
                            (int)p.GetImageHeight() * (int)p.GetScaleY())), counter, 2);
                //Checkpoint4 detection
                Checkpoint(new Point(400, 246), new Size(215, 33),
                    new Rectangle(new Point((int)newX, (int)newY),
                        new Size((int)p.GetImageWidth() * (int)p.GetScaleX(),
                            (int)p.GetImageHeight() * (int)p.GetScaleY())), counter, 3);
                //Checkpoint5 detection
                Checkpoint(new Point(41, 180), new Size(170, 33),
                    new Rectangle(new Point((int)newX, (int)newY),
                        new Size((int)p.GetImageWidth() * (int)p.GetScaleX(),
                            (int)p.GetImageHeight() * (int)p.GetScaleY())), counter, 4);

                //Pitstop detection
                Pitstop(counter, new Rectangle(new Point((int)newX, (int)newY), new Size((int)p.GetImageWidth() * (int)p.GetScaleX(), (int)p.GetImageHeight() * (int)p.GetScaleY())));
                
                counter++;
            }

            //tell the window to redraw the canvas
            Invalidate();
        }

        /// <summary>
        /// The loop that updates the player information shown on screen
        /// </summary>
        /// <param name="sender">Is not used and will stay null, only in place because a Timer requires it</param>
        /// <param name="e">Is not used and will stay null, only in place because a Timer requires it</param>
        private void InfoUpdate(object sender, EventArgs e)
        {
            //Reset the playerNumber to use the range 1 and 2
            int count = 1;

            //The loop that updates player 1 and player 2 information like current speed and fuel
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

            //Updates the player information on screen
            Meter1.Text = player1Name + ":\r\nSpeed: " + (int)Player1Info[0]*12 + "\r\nFuel: " + (int)Player1Info[1] + "\r\nLaps: " + (int)Player1Info[2] + "/" + (int)Player1Info[4] + "\r\nPits: " + (int)Player1Info[3];
            Meter2.Text = ":" + player2Name + "\r\n" + (int)Player2Info[0]*12 + " :Speed\r\n" + (int)Player2Info[1] + " :Fuel\r\n" + (int)Player2Info[2] + "/" + (int)Player1Info[4] + " :Laps\r\n" + (int)Player2Info[3] + " :Pits";
        }

        /// <summary>
        /// Keys up delegate used to update the current input and also update the player inputs
        /// </summary>
        /// <param name="sender">Is not used and will stay null, only in place because the delegate requires it</param>
        /// <param name="e">Current key up event</param>
        private void SetKeysUp(object sender, KeyEventArgs e)
        {
            //Compare the current key up event with the current inputs
            if (currentInput.Contains(e.KeyCode))
            {
                foreach (Player p in players)
                {
                        //Update the player input
                        p.KeyLetGo(e.KeyCode);
                }
                currentInput.Remove(e.KeyCode);
            }
        }

        /// <summary>
        /// Keys down delegate used to update the current input
        /// </summary>
        /// <param name="sender">Is not used and will stay null, only in place because the delegate requires it</param>
        /// <param name="e">Current key down event</param>
        private void SetKeysDown(object sender, KeyEventArgs e)
        {
            //If the current inputs does not contain the current key down add it to the current inputs
            if (!currentInput.Contains(e.KeyCode))
            {
                currentInput.Add(e.KeyCode);
            }
        }

        /// <summary>
        /// Function called when the window closes
        /// </summary>
        /// <param name="sender">Is not used and will stay null, only in place because the window requires it</param>
        /// <param name="e">Is not used and will stay null, only in place because the window requires it</param>
        private void Exit(object sender, FormClosedEventArgs e)
        {
            //Force to redraw the screen to make it shutdown gracefully
            Invalidate();
        }

        /// <summary>
        /// Compares all the current inputs to all the players
        /// </summary>
        private void SendInputs()
        {
            //Loop over all players
            foreach (Player p in players)
            {
                //Compare all current inputs
                foreach (Keys k in _currentInput)
                {
                    p.CompareInput(k);
                }
            }
        }

        /// <summary>
        /// Detects if the player is within the finish area
        /// </summary>
        /// <param name="playerNr">The player to compare</param>
        /// <param name="rectToCompare">The Rectangle to compare</param>
        /// <returns></returns>
        public bool Finish(int playerNr, Rectangle rectToCompare)
        {
            //Rectangle holding the area where the finish resides
            Rectangle finishRect = new Rectangle(finishPoint.X, finishPoint.Y, finishPoint.Width,  finishPoint.Height);

            //Compare if the player is within the finish area
            if (finishRect.Contains(rectToCompare))
            {
                players[playerNr].Finish();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Detects if the player is within the pit stop area
        /// </summary>
        /// <param name="playerNr">The player to refuel</param>
        /// <param name="rectToCompare">The Rectangle containing the player</param>
        /// <returns></returns>
        public bool Pitstop(int playerNr,Rectangle rectToCompare)
        {
            //Rectangle holding the area where the pitstop resides
            Rectangle pitRect = new Rectangle(pitStopPoint.X, pitStopPoint.Y, pitStopPoint.Width, pitStopPoint.Height);

            //Compares if the player is within the pitstop area
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


        /// <summary>
        /// Checks if the rectToCompare is within the checkpoint rectangle and add it to the players
        /// </summary>
        /// <param name="pointPos">The checkpoint</param>
        /// <param name="checkpointSize">The width and the height of the area</param>
        /// <param name="rectToCompare">The rectangle to compare to the checkpoint</param>
        /// <param name="playerNr">The number of the player to compare</param>
        /// <param name="checkpoint">Value used to set when within the checkpoint (0 = not passed, 1 = passed)</param>
        /// <returns>Returns true if the player is within the checkpoint</returns>
        public bool Checkpoint(Point pointPos,Size checkpointSize, Rectangle rectToCompare,int playerNr, int checkpoint)
        {

            int width = checkpointSize.Width;
            int height = checkpointSize.Height;

            Rectangle pitRect = new Rectangle(pointPos.X, pointPos.Y, width, height);

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

