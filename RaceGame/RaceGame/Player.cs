using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Windows.Forms;
using RaceGame.Properties;

namespace RaceGame
{
    /// <summary>
    /// Class used to make multiple players and hold individual information
    /// </summary>
    class Player
    {

        /// <summary>
        /// Amount of times refueled
        /// </summary>
        public int timesRefueled = 0;

        /// <summary>
        /// Amount of rounds passed by this player
        /// </summary>
        int roundsElapsed = 0;
        
        /// <summary>
        /// Amount of fuel remaining
        /// </summary>
        float fuelRemaining = 100;
        
        /// <summary>
        /// Amount of times refueled
        /// </summary>
        int pitStops = 0;

        /// <summary>
        /// Boolean indicating the forward key is pressed
        /// </summary>
        bool F;
        /// <summary>
        /// Boolean indicating the backward key is pressed
        /// </summary>
        bool B;
        /// <summary>
        /// Boolean ind icating the left key is pressed
        /// </summary>
        bool L;
        /// <summary>
        /// Boolean indicating the right key is pressed
        /// </summary>
        bool R;
        
        /// <summary>
        /// Timer for draining the fuel
        /// </summary>
        int fuelCalcCounter = 0;

        /// <summary>
        /// Used to calculate an average fuel consumption
        /// </summary>
        float[] fuelCalcVal = new float[10];

        /// <summary>
        /// Indicates that the maxspeed should be halved
        /// </summary>
        bool fuelSlow = false;
        
        /// <summary>
        /// Indicates if the player can control the car
        /// </summary>
        bool canMove = true;

        /// <summary>
        /// Indicates if the player is on the grass
        /// </summary>
        bool grassSlow = false;

        /// <summary>
        /// Holds all the checkpoints needed to increase rounds
        /// </summary>
        int[] Checkpoints = new int[5] {0,0,0,0,0};

        /// <summary>
        /// The color of the pixel underneath the player
        /// </summary>
        Color pixelColor;
        
        /// <summary>
        /// Instance of the player car
        /// </summary>
        Car playerCar;

        /// <summary>
        /// The name of the player
        /// </summary>
        string playerName;

        /// <summary>
        /// Id of the player
        /// </summary>
        string name;

        /// <summary>
        /// All the keys the car can respond to
        /// </summary>
        List<Keys> playerKeys;

        /// <summary>
        /// Initializes a new Player
        /// </summary>
        /// <param name="id">The id of the player</param>
        /// <param name="playerName">The name of the player</param>
        /// <param name="startPos">The start position of the car</param>
        /// <param name="startRot">The start rotation of the car</param>
        /// <param name="playerImage">The image used as a car</param>
        /// <param name="playerKeysToUse">All the keys that work with this player</param>
        public Player(string id,string playerName, Point startPos, int startRot, Bitmap playerImage, List<Keys> playerKeysToUse)
        {

            this.name = id;
            this.playerName = playerName;
            
            //Assign a car to each player
            this.playerCar = new Car(int.Parse(name),startPos,startRot,playerImage,0.25f,0.25f);
            this.playerKeys = playerKeysToUse;
            
            //Register with graphicsEngine
            GraphicsEngine.AddAsset(new Asset(++GraphicsEngine.assetsToRender,playerCar.image,playerCar.pos,playerCar.rot,playerCar.scaleX,playerCar.scaleY),RenderType.Player);

            //Player control timer
            Timer playerTimer = new Timer();
            playerTimer.Interval = 1;
            playerTimer.Tick += Timer_Tick;
            playerTimer.Start();

            //Player event timer
            Timer EventTimer = new Timer();
            EventTimer.Interval = 10;
            EventTimer.Tick += Event_Tick;
            EventTimer.Start();

        }

        /// <summary>
        /// Method for passing info to the realtime displays
        /// </summary>
        /// <returns>Returns currentSpeed, fuelRemaining, roundsElapsed, timesRefueled, amount of rounds to race</returns>
        public double[] GetInfo()
        {
            return new double[5] {playerCar.currentSpeed, fuelRemaining, roundsElapsed, timesRefueled, MainWindow.requiredRounds};
        }
        
        /// <summary>
        /// Method for geting the position of the car
        /// </summary>
        /// <returns>Player car position</returns>
        public Point GetCarPos()
        {
            return playerCar.pos;
        }

        /// <summary>
        /// Method for getting the rotation of the car
        /// </summary>
        /// <returns>Player car rotation</returns>
        public float GetCarRot()
        {
            return playerCar.rot;
        }

        /// <summary>
        /// Function for getting the x scale of the car image
        /// </summary>
        /// <returns>Scale X</returns>
        public float GetScaleX()
        {
            return playerCar.scaleX;
        }

        /// <summary>
        /// Function for getting the y scale of the car image
        /// </summary>
        /// <returns>Scale Y</returns>
        public float GetScaleY()
        {
            return playerCar.scaleY;
        }

        /// <summary>
        /// Function for getting the width of the image
        /// </summary>
        /// <returns></returns>
        public int GetImageWidth()
        {
            return playerCar.image.Width;
        }

        /// <summary>
        /// Function for getting the height of the image
        /// </summary>
        /// <returns></returns>
        public int GetImageHeight()
        {
            return playerCar.image.Height;
        }

        /// <summary>
        /// Refueles the car if the fuel is less than max 
        /// </summary>
        public void Refuel()
        {
            if (fuelRemaining<100)
            {
                playerCar.currentSpeed = 2;
                playerCar.Decellerate();
                playerCar.Decellerate();
                fuelRemaining++;

                canMove = false;
            }
            if (fuelRemaining >= 100 && !canMove)
            {
                canMove = true;
                timesRefueled++;
            }
        }

        /// <summary>
        /// Sets checkpoint number 'checkpoint' to 1
        /// </summary>
        /// <param name="checkpoint">Number of the checkpoint to change</param>
        public void Checkpoint(int checkpoint)
        {
            Checkpoints[checkpoint] = 1;
        }
        
        /// <summary>
        /// Method with actions that are to be performed on crossing the finish line
        /// </summary>
        public void Finish()
        {
            int check = 0;

            //Checked all waypoints past
            foreach (int i in Checkpoints)
            {
                if (i == 1)
                {
                    check++;
                }
            }

            //If all waypoints passed add a round
            if (check == 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    Checkpoints[i] = 0;
                }

                roundsElapsed++;
            }

            //Open winner window when rounds is 3
            if (roundsElapsed == MainWindow.requiredRounds)
            {
                MainWindow.ActiveForm.Hide();
                FinishWindow Finish = new FinishWindow(playerName);
                Finish.Closed += (s, args) => MainWindow.ActiveForm.Close();
                Finish.Show();
            }

        }

        /// <summary>
        /// Compare keyToCompare to the allowed keys and set correct boolean to true
        /// </summary>
        /// <param name="keyToCompare">The key to compare to the players keys</param>
        public void CompareInput(Keys keyToCompare)
        {
            if (!playerKeys.Contains(keyToCompare))
            {
                return;    
            }
            if (playerKeys[0] == keyToCompare)
            {
                F = true;
            }
            if (playerKeys[1] == keyToCompare)
            {
                B = true;
            }
            if (playerKeys[2] == keyToCompare)
            {
                L = true;
            }
            if (playerKeys[3] == keyToCompare)
            {
                R = true;
            }
        }
        /// <summary>
        /// Compare keyToCompare to the allowed keys and set correct boolean to false
        /// </summary>
        /// <param name="keyToCompare">The key to compare to the players keys</param>
        public void KeyLetGo(Keys keytoRemove)
        {
            if (playerKeys[0] == keytoRemove)
            {
                F = false;
            }
            if (playerKeys[1] == keytoRemove)
            {
                B = false;
            }
            if (playerKeys[2] == keytoRemove)
            {
                L = false;
            }
            if (playerKeys[3] == keytoRemove)
            {
                R = false;
            }
        }

        //tick for moving the car
        
        /// <summary>
        /// Main loop to move the care
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            //If the car can't move keep adding fuel
            if (!canMove) {  Refuel();   return; }

                if (F)
                {
                    playerCar.Accelerate('F');
                }
                if (L)
                {
                    if (playerCar.currentSpeed != 0)
                    {
                        playerCar.SteerLeft();
                    }
                }
                if (B)
                {
                    playerCar.Accelerate('B');
                }
                if (R)
                {
                    if (playerCar.currentSpeed != 0)
                    {
                        playerCar.SteerRight();
                    }
                }
                if (!F && !B)
                {
                    if (playerCar.currentSpeed != 0)
                    {
                        playerCar.Decellerate();
                    }
                }
                if (playerCar.currentSpeed > playerCar.maxSpeed)
                {
                    playerCar.Decellerate();
                }
        }

        /// <summary>
        /// Tick for fuel and grass detection
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
        private void Event_Tick(object sender, EventArgs e)
        {
            //If the fuel counter is less than 10 create a average array
            if (fuelCalcCounter < 10)
            {
                if (playerCar.currentSpeed < 0)
                {
                    fuelCalcVal[fuelCalcCounter] = playerCar.currentSpeed * -1 / playerCar.maxSpeed;
                }
                else
                {
                    fuelCalcVal[fuelCalcCounter] = playerCar.currentSpeed / playerCar.maxSpeed;
                }
                fuelCalcCounter++;
            }
            //if the fuel counter is 10 reduce the fuel by an average of the fuelCalcVal array
            else if (fuelCalcCounter == 10)
            {
                float avg = 0;

                foreach (float val in fuelCalcVal)
                {
                    avg += val;
                }
                avg /= 10;

                if (fuelRemaining > 0)
                {
                    fuelRemaining -= avg * 2.5f;
                }
                if (fuelRemaining <0)
                {
                    fuelRemaining = 0; 
                }

                fuelCalcCounter = 0;

                if (playerCar.currentSpeed < 0)
                {
                    fuelCalcVal[fuelCalcCounter] = playerCar.currentSpeed * -1 / playerCar.maxSpeed;
                }
                else
                {
                    fuelCalcVal[fuelCalcCounter] = playerCar.currentSpeed / playerCar.maxSpeed;
                }
            }

            //If the fuel is drained reduce the maxSpeed
            if (fuelRemaining <= 0 && fuelSlow == false)
            {
                if (grassSlow)
                {
                    playerCar.maxSpeed = 2.5f;
                }
                else
                {
                    playerCar.maxSpeed = 5f;
                }
                fuelSlow = true;
            }
            //If the player is on the grass reduce maxSpeed
            else if (fuelRemaining > 0 && fuelSlow)
            {
                switch (grassSlow)
                {
                    case true:
                        playerCar.maxSpeed = 5f;
                        break;
                    case false:
                        playerCar.maxSpeed = 10;
                        break;
                }
                fuelSlow = false;
            }

            //Creates a copy of the background with the same size to compare
            Bitmap BackgroundImage = new Bitmap(Resources.Background, MainWindow.screenSize.Width, MainWindow.screenSize.Height);

            //Detection of the color of the pixels under the car
            pixelColor = BackgroundImage.GetPixel((int)(GetCarPos().X * GetScaleX() + (GetImageWidth() / 2) * GetScaleX()),
                (int)(GetCarPos().Y * GetScaleY() + (GetImageHeight() / 2) * GetScaleY()));

            //If the player is on the grass and the fuel is drained reduce maxSpeed
            if (pixelColor == Color.FromArgb(0, 0, 0, 0) && grassSlow == false)
            {
                if (fuelSlow)
                {
                    playerCar.maxSpeed = 2.5f;
                }
                else
                {
                    playerCar.maxSpeed = 5f;
                }
                grassSlow = true;
            }

            //If the player is going back to the road and the fuel is drained increase the maxSpeed
            else if (pixelColor != Color.FromArgb(0, 0, 0, 0) && grassSlow)
            {
                switch (fuelSlow)
                {
                    case true:
                        playerCar.maxSpeed = 5f;
                        break;
                    case false:
                        playerCar.maxSpeed = 10;
                        break;
                }
                grassSlow = false;
            }
        }

    }
}