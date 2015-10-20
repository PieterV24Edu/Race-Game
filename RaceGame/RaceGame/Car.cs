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
        public float scaleX,scaleY; 

        bool moving = false;

        int playerId;
        int maxSpeed =20;
        Timer carTimer = new Timer();
        Timer brakeTimer = new Timer();


        public Car(int playerId, Point startPos, int startRot, Bitmap carImage)
        {
            this.playerId = playerId;
            pos = startPos;
            rot = startRot;
            
            image = carImage;

            scaleX = 1;
            scaleY = 1;

            carTimer.Interval = 1;
            carTimer.Elapsed += MoveCar;
            carTimer.Start();
        }

        private void MoveCar(object sender, ElapsedEventArgs e)
        {

            pos.X += currentSpeed;

            if (pos.X  >= MainWindow.screenSize.Width)
            {
                pos.X = MainWindow.screenSize.Width;
            }

            if (pos.X <= 1)
            {
                pos.X = 1;
            }

            GraphicsEngine.UpdatePos(playerId, pos);
        }

        public void Accelerate(int speed)
        {   
            brakeTimer.Stop();

            currentSpeed += speed;
        }

        public void Brake()
        {

            brakeTimer.Interval = 10;

            brakeTimer.Elapsed += new ElapsedEventHandler
                (
                    (o, b) =>
                        {
                            if (currentSpeed >= 1)
                            {
                                currentSpeed--;
                            }

                            if (currentSpeed <= -1)
                            {
                                currentSpeed++;
                            }

                            if (currentSpeed == 0)
                            {
                                brakeTimer.Stop();
                            }
                        }
                );

            brakeTimer.Start();
        }

        internal void SteerLeft()
        {
            rot -= 10;
            GraphicsEngine.UpdateRot(playerId,rot);
        }

        internal void SteerRight()
        {
            rot += 10;
            GraphicsEngine.UpdateRot(playerId, rot);
        }

        //insert wheels
        //insert acceleration
        //insert rotation
        //insert brakes
        //insert revers



    }
}
