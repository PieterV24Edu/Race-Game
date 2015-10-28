﻿using System;
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
        public float currentSpeed = 0;
        public int rot;
        public float scaleX,scaleY;

        int playerId;
        public int maxSpeed =12;
        public float accStep = 0.5f;
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
        }

        private void MoveCar(object sender, ElapsedEventArgs e)
        {
            pos = CalcMovePoint(currentSpeed, rot);
            /*if (pos.X < 0)
            {
                pos.X = 0;
            }
            if (pos.X > 1011)
            {
                pos.X = 1011;
            }
            if (pos.Y < 0)
            {
                pos.Y = 0;
            }
            if (pos.Y > 729)
            {
                pos.Y = 729;
            }*/
            GraphicsEngine.UpdatePos(playerId, pos);
        }

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

        public void SteerLeft()
        {
            rot -= 10;
            Decellerate();
            GraphicsEngine.UpdateRot(playerId,rot);
        }

        public void SteerRight()
        {
            rot += 10;
            Decellerate();
            GraphicsEngine.UpdateRot(playerId, rot);
        }

        double DegtoRad(double deg)
        {
            return (Math.PI/180)*deg;
        }

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
