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
    /// <summary>
    /// Class used to hold car information and functions
    /// </summary>
    class Car
    {
        /// <summary>
        /// The car image
        /// </summary>
        public Bitmap image;

        /// <summary>
        /// The car position
        /// </summary>
        public Point pos;

        /// <summary>
        /// The current speed of the car
        /// </summary>
        public float currentSpeed = 0;
        /// <summary>
        /// The rotation of the car
        /// </summary>
        public int rot;

        /// <summary>
        /// The scale of the car
        /// </summary>
        public float scaleX,scaleY;

        /// <summary>
        /// The actual size of the car
        /// </summary>
        Point imageSize;

        /// <summary>
        /// The id of the player
        /// </summary>
        int playerId;

        /// <summary>
        /// The maximum speed the car can travel at
        /// </summary>
        public float maxSpeed =10;

        /// <summary>
        /// Increments for accelleration and decelleration of the car
        /// </summary>
        public float accStep = 1f;

        /// <summary>
        /// Timer used as loop for the car movement
        /// </summary>
        Timer carTimer = new Timer();

        /// <summary>
        /// Initializes a new Car
        /// </summary>
        /// <param name="playerId">The id of the player</param>
        /// <param name="startPos">The start position of the car</param>
        /// <param name="startRot">The start rotation of the car</param>
        /// <param name="carImage">The image used as car</param>
        /// <param name="xScale">The x scale of the car</param>
        /// <param name="yScale">The y scale of the car</param>
        public Car(int playerId, Point startPos, int startRot, Bitmap carImage,float xScale=0,float yScale=0)
        {
            this.playerId = playerId;
            pos = startPos;
            rot = startRot;
            
            image = carImage;

            //If the xScale is not assigned set scaleX to 1
            if (xScale == 0)
            {
                scaleX = 1;
            }
            else
            {
                scaleX = xScale;
            }

            //If the yScale is not assigned set scaleY to 1
            if (yScale == 1)
            {
                scaleY = 1;
            }
            else
            {
                scaleY = yScale;
            }

            //Initialize the movement loop
            carTimer.Interval = 1;
            carTimer.Elapsed += MoveCar;
            carTimer.Start();

            //Scale the new car 
            GraphicsEngine.UpdateScale(playerId,scaleX,scaleY);

            //Set the unscaled size of the image 
            imageSize = new Point(image.Width, image.Height);
        }

        /// <summary>
        /// Moves the car by calculating a new point useing the speed and the rotation
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
        private void MoveCar(object sender, ElapsedEventArgs e)
        {
            pos = CalcMovePoint(currentSpeed, rot);
            //Check if car is not going outside of the window
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
            //Update position of the car using the GraphicsEngine class
            GraphicsEngine.UpdatePos(playerId, pos);
        }

        /// <summary>
        /// Acceleration method with switch to identify forward and backward acceleration
        /// </summary>
        /// <param name="dir">F or B</param>
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
        
        /// <summary>
        /// Decelleration method with check if decelleration should be aplied on forward or backward momentum
        /// </summary>
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
        
        /// <summary>
        /// Steering left method
        /// </summary>
        public void SteerLeft()
        {
            rot -= 15;
            Decellerate();
            GraphicsEngine.UpdateRot(playerId,rot);
        }

        /// <summary>
        /// Steering right method
        /// </summary>
        public void SteerRight()
        {
            rot += 15;
            Decellerate();
            GraphicsEngine.UpdateRot(playerId, rot);
        }

        /// <summary>
        /// Method to convert angle in deg to angel in rad
        /// </summary>
        /// <param name="deg">Degrees to convert</param>
        /// <returns>Radians</returns>
        double DegtoRad(double deg)
        {
            return (Math.PI/180)*deg;
        }
        
        /// <summary>
        /// Move logic. formula that calculates the movement on the x and y axis depending on the speed and angle of the car
        /// </summary>
        /// <param name="speed">The speed of the car</param>
        /// <param name="angle">The angle of the car</param>
        /// <returns></returns>
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
