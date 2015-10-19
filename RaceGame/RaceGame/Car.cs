using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;

namespace RaceGame
{
    class Car
    {
        public Bitmap image;
        public Point pos;
        public int currentSpeed = 0;
        public int rot;
        public Size carScale; 

        bool moving = false;

        int playerId;
        int maxSpeed = 7;
        Timer carTimer = new Timer();

        public Car(int playerId, Point startPos, int startRot, Bitmap carImage)
        {
            this.playerId = playerId;
            pos = startPos;
            rot = startRot;
            
            image = carImage;

            carScale = new Size(1,1);
            
            carTimer.Interval = 1;
            carTimer.Elapsed += MoveCar;
            carTimer.Start();
        }

        private void MoveCar(object sender, ElapsedEventArgs e)
        {

            pos.X += currentSpeed;

            if (pos.X  >= MainWindow.width)
            {
                pos.X = MainWindow.width;
            }

            if (pos.X <= 1)
            {
                pos.X = 1;
            }

            GraphicsEngine.UpdatePos(playerId, pos);
        }

        public void Accelerate(int speed)
        {
            

            //front
            if (currentSpeed <= maxSpeed && speed > 0)
            {
                currentSpeed += speed;
            }

            //back
            if (currentSpeed >= -maxSpeed && speed < 0)
            {
                currentSpeed += speed;
            }




        }

        public void Brake()
        {
            while (currentSpeed != 0)
            {

                if (currentSpeed > 1)
                {
                    currentSpeed--;
                    GraphicsEngine.UpdatePos(playerId, pos);
                }

                if (currentSpeed < -1)
                {
                    currentSpeed++;
                    GraphicsEngine.UpdatePos(playerId, pos);
                }
            }
        }

        void SlowDown(object sender, ElapsedEventArgs e)
        {

            while (currentSpeed != 0)
            {
                
                if (currentSpeed > 1)
                {
                    currentSpeed--;
                    GraphicsEngine.UpdatePos(playerId, pos);
                    return;
                }

                if (currentSpeed < -1)
                {
                    currentSpeed++;
                    GraphicsEngine.UpdatePos(playerId, pos);
                    return;
                }
            }
        }

        internal void SteerLeft()
        {
        }

        internal void SteerRight()
        {
        }

        //insert wheels
        //insert acceleration
        //insert rotation
        //insert brakes
        //insert revers



    }
}
