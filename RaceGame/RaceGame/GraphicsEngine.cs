using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using RaceGame.Properties;
using Timer = System.Threading.Timer;

namespace RaceGame
{
    enum RenderType
    {
        Player,
        Props
    }

    class GraphicsEngine
    {
        public static int assetsToRender = 0;

        public int frames;
        public int startTime;

        Graphics graphicsBuffer;
        Graphics drawHandle;
        Bitmap backBuffer;
        
        static List<Asset> playerAssets = new List<Asset>();
        static List<Asset> propAssets = new List<Asset>();
        Point newPointOfAsset;

        public GraphicsEngine(Graphics dHandle)
        {
            drawHandle = dHandle;
        }

        public void GraphicsUpdate(object o)
        {


            backBuffer = new Bitmap(MainWindow.screenSize.Width, MainWindow.screenSize.Height);
            graphicsBuffer = Graphics.FromImage(backBuffer);

            graphicsBuffer.ResetTransform();

                if (graphicsBuffer != null)
                {
                    graphicsBuffer.Clear(Color.Green);                    
                }

                Bitmap temp = new Bitmap(Resources.Background, MainWindow.screenSize.Width, MainWindow.screenSize.Height);

                graphicsBuffer.DrawImage(temp, new PointF(0, 0));
                temp.Dispose();

                PlayerThread();

                drawHandle.DrawImage(backBuffer, 0,0);
                frames++;

                //if (Environment.TickCount >= startTime + 1000)
                //{
                //    Debug.Print("FPS: " + frames);
                //    frames = 0;
                //    startTime = Environment.TickCount;
                //}

        }

        public void PlayerThread()
        {
                for (int i = 0; i < playerAssets.Count; i++)
                {
                    Matrix rotate = new Matrix();

                    newPointOfAsset.X = Convert.ToInt32(playerAssets[i].pointOfAsset.X + (playerAssets[i].imageToDisplay.Width / 2));
                    newPointOfAsset.Y = Convert.ToInt32(playerAssets[i].pointOfAsset.Y + (playerAssets[i].imageToDisplay.Height / 2));

                    rotate.Scale(playerAssets[i].scaleX,playerAssets[i].scaleY);
                    rotate.RotateAt(playerAssets[i].rotationOfAsset,newPointOfAsset);
                    
                    graphicsBuffer.Transform = rotate;

                    graphicsBuffer.DrawImage(playerAssets[i].imageToDisplay, playerAssets[i].pointOfAsset);
                }

        }

        public void PropsThread()
        {
                for (int i = 0; i < propAssets.Count; i++)
                {
                    Bitmap tempImage = new Bitmap(propAssets[i].imageToDisplay);
                    graphicsBuffer.DrawImage(tempImage, propAssets[i].pointOfAsset);
                    tempImage.Dispose();
                }
        }

        public static void AddAsset(Asset assetToRender, RenderType type)
        {
                switch (type)
                {
                        case RenderType.Player:
                            playerAssets.Add(assetToRender);
                            //add on top of players
                            break;
                        case RenderType.Props:
                            propAssets.Add(assetToRender);
                            //add on top of props
                            break;
                }
        }

        public static void UpdatePos(int assetId , Point pos)
        {

            for (int i = 0; i < playerAssets.Count; i++)
            {
                if (playerAssets[i].assetId == assetId)
                {
                    playerAssets[i].pointOfAsset = pos;
                }
            }
        }

        public static void UpdateRot(int assetId, int rot)
        {

            for (int i = 0; i < playerAssets.Count; i++)
            {
                if (playerAssets[i].assetId == assetId)
                {
                    playerAssets[i].rotationOfAsset = rot;
                }
            }
        }

        public static void UpdateScale(int assetId, float scaleX,float scaleY)
        {

            for (int i = 0; i < playerAssets.Count; i++)
            {
                if (playerAssets[i].assetId == assetId)
                {
                    playerAssets[i].scaleX = scaleX;
                    playerAssets[i].scaleY = scaleY;
                }
            }
        }
    }
}
