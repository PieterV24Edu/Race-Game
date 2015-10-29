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
    class Player
    {
        string name;
        
        int roundsElapsed = 0;
        float fuelRemaining = 100;
        int pitStops = 0;
        bool F, B, L, R;
        int fuelCalcCounter = 0;
        float[] fuelCalcVal = new float[10];
        bool fuelSlow = false;
        bool canMove = true;
        bool grassSlow = false;
        int[] Checkpoints = new int[5] {0,0,0,0,0};

        Color pixelColor;

        Car playerCar;
        string playerName;
        List<Keys> playerKeys;

        public Player(string name,string playerName, Point startPos, int startRot, Bitmap playerImage, List<Keys> playerKeysToUse)
        {
            this.name = name;
            this.playerName = playerName;
            //assign a car to each player
            this.playerCar = new Car(int.Parse(name),startPos,startRot,playerImage,0.25f,0.25f);
            this.playerKeys = playerKeysToUse;
            //register with graphicsEngine
            GraphicsEngine.AddAsset(new Asset(++GraphicsEngine.assetsToRender,playerCar.image,playerCar.pos,playerCar.rot,playerCar.scaleX,playerCar.scaleY),RenderType.Player);

            //player control timer
            Timer playerTimer = new Timer();
            playerTimer.Interval = 1;
            playerTimer.Tick += Timer_Tick;
            playerTimer.Start();

            //player event timer
            Timer EventTimer = new Timer();
            EventTimer.Interval = 10;
            EventTimer.Tick += Event_Tick;
            EventTimer.Start();

        }
        //method for passign info to the realtime displays
        public double[] GetInfo()
        {
            return new double[5] {playerCar.currentSpeed, fuelRemaining, roundsElapsed, pitStops, MainWindow.requiredRounds};
        }
        //method for geting the position of the car
        public Point GetCarPos()
        {
            return playerCar.pos;
        }
        //method for getting the rotation of the car
        public float GetCarRot()
        {
            return playerCar.rot;
        }
        //methods for getting scale and size of the car
        public float GetScaleX()
        {
            return playerCar.scaleX;
        }

        public float GetScaleY()
        {
            return playerCar.scaleY;
        }

        public int GetImageWidth()
        {
            return playerCar.image.Width;
        }
        public int GetImageHeight()
        {
            return playerCar.image.Height;
        }
        //refueling function
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
            if (fuelRemaining >= 100)
            {
                canMove = true;
            }
        }
        //checkpoint registry
        public void Checkpoint(int checkpoint)
        {
            Checkpoints[checkpoint] = 1;
        }
        //method with actions that are to be performed on crossing the finish line
        //first it is checked if you have passed all the waypoints, if so a round is added
        //when the amount of rounds is equal to the amount of required rounds the final window is displayed with the name of the winner 
        public void Finish()
        {
            int check = 0;
            foreach (int i in Checkpoints)
            {
                if (i == 1)
                {
                    check++;
                }
            }
            if (check == 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    Checkpoints[i] = 0;
                }
                roundsElapsed++;
            }
            //open winner window
            if (roundsElapsed == MainWindow.requiredRounds)
            {
                MainWindow.ActiveForm.Hide();
                FinishWindow Finish = new FinishWindow(playerName);
                Finish.Closed += (s, args) => MainWindow.ActiveForm.Close();
                Finish.Show();
            }
        }
        //registering keyinputs
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
        //registering keyreleases
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
        private void Timer_Tick(object sender, EventArgs e)
        {
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
        //tick for fuel and grass detection
        private void Event_Tick(object sender, EventArgs e)
        {
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
            //detection of the color of the pixels under the car
            Bitmap BackgroundImage = new Bitmap(Resources.Background, MainWindow.screenSize.Width, MainWindow.screenSize.Height);
            pixelColor = BackgroundImage.GetPixel((int)(GetCarPos().X * GetScaleX() + (GetImageWidth() / 2) * GetScaleX()),
                (int)(GetCarPos().Y * GetScaleY() + (GetImageHeight() / 2) * GetScaleY()));
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