using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using RaceGame.Properties;
using Timer = System.Timers.Timer;

namespace RaceGame
{
    class Car
    {
        public Bitmap image;
        public Point pos;
        public float currentSpeed = 0;
        public int rot;
        public float scaleX,scaleY;
        Point imageSize;

        int playerId;
        //maximum speed the car can travel at
        public float maxSpeed =10;
        //increments for accelleration and decelleration of the car
        public float accStep = 1f;
        Timer carTimer = new Timer();

        public Car(int playerId, Point startPos, int startRot, Bitmap carImage,float xScale=0,float yScale=0)
        {
            this.playerId = playerId;
            pos = startPos;
            rot = startRot;
            
            image = carImage;

            if (xScale == 0)
            {
                scaleX = 1;
            }
            else
            {
                scaleX = xScale;
            }

            if (yScale == 1)
            {
                scaleY = 1;
            }
            else
            {
                scaleY = yScale;
            }

            carTimer.Interval = 1;
            carTimer.Elapsed += MoveCar;
            carTimer.Start();

            GraphicsEngine.UpdateScale(playerId,scaleX,scaleY);
            imageSize = new Point(image.Width, image.Height);
        }

        private void MoveCar(object sender, ElapsedEventArgs e)
        {
            pos = CalcMovePoint(currentSpeed, rot);
            //check if car is not going outside of the window
            if (pos.X < 0)
            {
                pos.X = 0;
            }
            if (pos.X > MainWindow.screenSize.Width * (1 / scaleX) - imageSize.X)
            {
                pos.X = (int)(MainWindow.screenSize.Width * (1 / scaleX) - imageSize.X);
            }

            if (pos.Y < 0)
            {
                pos.Y = 0;
            }
            if (pos.Y > MainWindow.screenSize.Height * (1 / scaleY) - imageSize.Y)
            {
                pos.Y = (int)(MainWindow.screenSize.Height * (1 / scaleY) - imageSize.Y);
            }
            //update position of the car using the graphicsengine class
            GraphicsEngine.UpdatePos(playerId, pos);
        }
        //acceleration method with switch to identify forward and backward acceleration
        public void Accelerate(char dir)
        {
            switch (dir)
            {
                case 'F':
                    if (currentSpeed < maxSpeed)
                    {
                        currentSpeed += accStep;
                    }
                    break;
                case 'B':
                    if (currentSpeed > -maxSpeed)
                    {
                        currentSpeed -= accStep;
                    }
                    break;
            }
        }
        //decelleration method with check if decelleration should be aplied on forward or backward momentum
        public void Decellerate()
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= accStep;
            }
            if (currentSpeed < 0)
            {
                currentSpeed += accStep;
            }
        }
        //steering method
        public void SteerLeft()
        {
            rot -= 15;
            Decellerate();
            GraphicsEngine.UpdateRot(playerId,rot);
        }

        public void SteerRight()
        {
            rot += 15;
            Decellerate();
            GraphicsEngine.UpdateRot(playerId, rot);
        }
        //method to convert angle in deg to angel in rad
        double DegtoRad(double deg)
        {
            return (Math.PI/180)*deg;
        }
        //move logic. formula that calculates the movement on the x and y axis depending on the speed and angle of the car
        Point CalcMovePoint(double speed, double angle)
        {
            int py = Convert.ToInt32(speed * (Math.Sin(DegtoRad(angle))));
            int px = Convert.ToInt32(speed * (Math.Cos(DegtoRad(angle))));
            py += pos.Y;
            px += pos.X;

            return new Point(px,py);
        }
    }
}
