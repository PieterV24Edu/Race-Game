using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RaceGame
{
    class Player
    {
        string name;
        
        int speed;
        int roundElapsed = 0;

        bool F, B, L, R;
        
        Car playerCar;
        List<Keys> playerKeys;

        public Player(string name, Point startPos, int startRot, Bitmap playerImage, List<Keys> playerKeysToUse)
        {
            this.name = name;
            this.playerCar = new Car(int.Parse(name),startPos,startRot,playerImage);
            this.playerKeys = playerKeysToUse;

            //register with graphicsEngine
            GraphicsEngine.AddAsset(new Asset(int.Parse(name),playerCar.image,playerCar.pos,playerCar.rot,playerCar.carScale),RenderType.Player);
        
            Timer playerTimer = new Timer();
            playerTimer.Interval = 1;
            playerTimer.Tick += SetMovement;
            playerTimer.Start();

        }

        private void SetMovement(object sender, EventArgs e)
        {

            if (F)
            {
                //move forward
                playerCar.Accelerate(1);
            }
            if (B)
            {
                playerCar.Accelerate(-1);
                //move backward
            }
            if (L)
            {
                playerCar.SteerLeft();
                //steer left
            }
            if (R)
            {
                playerCar.SteerRight();
                //steer right
            }
        }

        public void CompareInput(Keys keyToCompare)
        {
            if (!playerKeys.Contains(keyToCompare)) { return; }

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

        public void KeyLetGo(Keys keyLetGo)
        {
            if (!playerKeys.Contains(keyLetGo)) { return;}

            if (playerKeys[0] == keyLetGo)
            {
                F = false;
            }
            if (playerKeys[1] == keyLetGo)
            {
                B = false;
            }
            if (playerKeys[2] == keyLetGo)
            {
                L = false;
            }
            if (playerKeys[3] == keyLetGo)
            {
                R = false;
            }
        }
    }
}
