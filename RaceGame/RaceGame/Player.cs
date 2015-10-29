﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
        float fuelRemaining = 90;
        int pitStops = 0;
        bool F, B, L, R;
        int fuelCalcCounter = 0;
        float[] fuelCalcVal = new float[10];
        bool fuelSlow = false;
        bool canMove = true;
        bool grassSlow = false;

        Color pixelColor;

        Car playerCar;
        List<Keys> playerKeys;
        private Image playImage;

        public Player(string name, Point startPos, int startRot, Bitmap playerImage, List<Keys> playerKeysToUse)
        {
            this.name = name;
            this.playerCar = new Car(int.Parse(name),startPos,startRot,playerImage,1f,1f);
            this.playerKeys = playerKeysToUse;
            this.playImage = playerImage;

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

        public double[] GetInfo()
        {
            return new double[4] {playerCar.currentSpeed, fuelRemaining, roundsElapsed, pitStops};
        }

        public Point GetCarPos()
        {
            return playerCar.pos;
        }

        public float GetCarRot()
        {
            return playerCar.rot;
        }

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

        public void Refuel()
        {

            if (fuelRemaining<100)
            {
                playerCar.currentSpeed = 2;

                fuelRemaining++;
                canMove = false;
            }
            if (fuelRemaining >= 100)
            {
                canMove = true;
            }

        }

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

            if (playerCar.currentSpeed > playerCar.maxSpeed)
            {
                playerCar.Decellerate();
            } 
        }

        private void Event_Tick(object sender, EventArgs e)
        {
            if (fuelCalcCounter < 10)
            {
                if (playerCar.currentSpeed < 0)
                {
                    fuelCalcVal[fuelCalcCounter] = playerCar.currentSpeed*-1/playerCar.maxSpeed;
                }
                else
                {
                    fuelCalcVal[fuelCalcCounter] = playerCar.currentSpeed/playerCar.maxSpeed;
                }
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
                    fuelRemaining -= avg*1.6f;
                }
                fuelCalcCounter = 0;
                if (playerCar.currentSpeed < 0)
                {
                    fuelCalcVal[fuelCalcCounter] = playerCar.currentSpeed*-1/playerCar.maxSpeed;
                }
                else
                {
                    fuelCalcVal[fuelCalcCounter] = playerCar.currentSpeed/playerCar.maxSpeed;
                }
            }



            
            if (fuelRemaining <= 0 && fuelSlow == false)
            {
                if (grassSlow)
                {
                    playerCar.maxSpeed = 1f;
                }
                else
                {
                    playerCar.maxSpeed = 1.5f;
                }
                fuelSlow = true;
            }
            else if (fuelRemaining > 0 && fuelSlow)
            {
                switch (grassSlow)
                {
                    case true:
                        playerCar.maxSpeed = 1.5f;
                        break;
                    case false:
                        playerCar.maxSpeed = 3;
                        break;
                }
                fuelSlow = false;
            }

            Bitmap BackgroundImage = new Bitmap(Resources.Background);
            pixelColor = BackgroundImage.GetPixel(GetCarPos().X + (int)(GetImageWidth()/ 2), GetCarPos().Y + (int)(GetImageHeight()/ 2));

            Debug.Print(playerCar.maxSpeed + "");

            if (pixelColor == Color.FromArgb(0, 0, 0, 0) && grassSlow == false)
            {
                if (fuelSlow)
                {
                    playerCar.maxSpeed = 1f;
                }
                else
                {
                    playerCar.maxSpeed = 1.5f;
                }
                grassSlow = true;
            }
            else if (pixelColor != Color.FromArgb(0, 0, 0, 0) && grassSlow)
            {
                switch (fuelSlow)
                {
                    case true:
                        playerCar.maxSpeed = 1.5f;
                        break;
                    case false:
                        playerCar.maxSpeed = 3;
                        break;
                }
                grassSlow = false;
            }
        }
    }
}