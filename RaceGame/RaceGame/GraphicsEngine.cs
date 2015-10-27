using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace RaceGame
{
    enum RenderType
    {
        Background,
        Player,
        Props
    }

    class GraphicsEngine
    {
        static object rendering = new object();
        public static int assetsToRender = 0;

        public int frames;
        public int startTime;

        Graphics graphicsBuffer;
        Graphics drawHandle;
        Bitmap backBuffer;
        Thread mainRenderThread;
        Timer debugThread;
        
        static List<Asset> backgroundAssets = new List<Asset>();
        static List<Asset> playerAssets = new List<Asset>();
        static List<Asset> propAssets = new List<Asset>();
        Point newPointOfAsset;

        public GraphicsEngine(Graphics dHandle)
        {
            drawHandle = dHandle;
        }

        public void Start()
        {
            mainRenderThread = new Thread(new ThreadStart(GraphicsUpdate));
            mainRenderThread.Start();
        }

        private void GraphicsUpdate()
        {
            backBuffer = new Bitmap(MainWindow.screenSize.Width, MainWindow.screenSize.Height);
            graphicsBuffer = Graphics.FromImage(backBuffer);

            while (true)
            {
                if (graphicsBuffer != null)
                {
                    graphicsBuffer.ResetTransform();
                }

                PlayerThread();

                drawHandle.DrawImage(backBuffer, 0,0);
                frames++;

            }
        }

        public void BackgroundThread()
        {
            lock (rendering)
            {
                graphicsBuffer.Clear(Color.Green);

                for (int i = 0; i < backgroundAssets.Count; i++)
                {

                    Bitmap tempImage = new Bitmap(backgroundAssets[i].imageToDisplay);

                    graphicsBuffer.DrawImage(backgroundAssets[i].imageToDisplay, backgroundAssets[i].pointOfAsset);

                    tempImage.Dispose();
                }
            }
        }

        public void PlayerThread()
        {
            lock (rendering)
            {

                for (int i = 0; i < playerAssets.Count; i++)
                {
                    Matrix rotate = new Matrix();

                    newPointOfAsset.X = Convert.ToInt32(playerAssets[i].pointOfAsset.X + (playerAssets[i].imageToDisplay.Width / 2));
                    newPointOfAsset.Y = Convert.ToInt32(playerAssets[i].pointOfAsset.Y + (playerAssets[i].imageToDisplay.Height / 2));

                    rotate.RotateAt(playerAssets[i].rotationOfAsset,newPointOfAsset);
                    
                    graphicsBuffer.Transform = rotate;

                    graphicsBuffer.ScaleTransform(playerAssets[i].scaleX, playerAssets[i].scaleY,MatrixOrder.Append);

                    graphicsBuffer.DrawImage(playerAssets[i].imageToDisplay, playerAssets[i].pointOfAsset);
                }
            }
        }

        public void PropsThread()
        {
            lock (rendering)
            {
                for (int i = 0; i < propAssets.Count; i++)
                {
                    Bitmap tempImage = new Bitmap(propAssets[i].imageToDisplay);
                    graphicsBuffer.DrawImage(tempImage, propAssets[i].pointOfAsset);
                    tempImage.Dispose();
                }
            }
        }

        public void Stop()
        {
            mainRenderThread.Abort();
        }

        public static void AddAsset(Asset assetToRender, RenderType type)
        {
            lock (rendering)
            {
                switch (type)
                {
                        case RenderType.Background:
                            backgroundAssets.Add(assetToRender);
                            //add on top of backgrounds
                            break;
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
