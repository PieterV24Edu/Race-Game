using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using RaceGame.Properties;

namespace RaceGame
{
    enum RenderType
    {
        Background,
        Player,
        Props,
        Info
    }


    class GraphicsEngine
    {
        static object shadowLock = new object();
        static object rendering = new object();
        public static int assetsToRender = 0;

        public int frames;
        public int startTime;

        Graphics backgroundBuffer;
        Graphics graphicsBuffer;
        Graphics drawHandle;
        Bitmap backBuffer;
        Thread mainRenderThread;
        Thread debugThread;
        
        static List<Asset> backgroundAssets = new List<Asset>();
        static List<Asset> playerAssets = new List<Asset>();
        static List<Asset> propAssets = new List<Asset>();
        static List<Asset> infoAssets = new List<Asset>();
        Point newPointOfAsset;

        public GraphicsEngine(Graphics dHandle)
        {
            drawHandle = dHandle;
        }

        public void Start()
        {
            mainRenderThread = new Thread(new ThreadStart(GraphicsUpdate));
            mainRenderThread.Start();

            debugThread = new Thread(new ThreadStart(DebugThread));
            debugThread.Start();

        }

        private void GraphicsUpdate()
        {
            backBuffer = new Bitmap(MainWindow.screenSize.Width, MainWindow.screenSize.Height);
            graphicsBuffer = Graphics.FromImage(backBuffer);

            while (true)
            {
                if (graphicsBuffer != null)
                {
                    graphicsBuffer.Clear(Color.Green);
                }

                graphicsBuffer.ResetTransform();

                BackgroundThread();
                PlayerThread();

                drawHandle.DrawImage(backBuffer, 0, 0);
                frames++;
            }
        }

        public void BackgroundThread()
        {
            lock (rendering)
            {

                for (int i = 0; i < backgroundAssets.Count; i++)
                {
                    Bitmap tempImage = new Bitmap(backgroundAssets[i].imageToDisplay);

                    float x = MainWindow.screenSize.Width;
                    float y = MainWindow.screenSize.Height;

                    x /= tempImage.Width;
                    y /= tempImage.Height;

                    x = Math.Abs(x);
                    y = Math.Abs(y);

                    graphicsBuffer.ScaleTransform(x, y);

                    graphicsBuffer.DrawImage(tempImage, backgroundAssets[i].pointOfAsset);

                    tempImage.Dispose();
                }
            }
        }

        public void PlayerThread()
        {
            lock (rendering)
            {
                graphicsBuffer.ResetTransform();

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

        public void InfoThread()
        {
            lock (rendering)
            {
                for (int i = 0; i < infoAssets.Count; i++)
                {
                    Bitmap tempImage = new Bitmap(infoAssets[i].imageToDisplay);
                    graphicsBuffer.DrawImage(tempImage, infoAssets[i].pointOfAsset);
                    tempImage.Dispose();
                }
            }
        }
        public void Stop()
        {
            mainRenderThread.Abort();
            debugThread.Abort();
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
                        case RenderType.Info:
                            infoAssets.Add(assetToRender);
                            //add on top of info
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
        
        public void DebugThread()
        {
            while (true)
            {

                if (Environment.TickCount >= startTime + 1000)
                {
                    Debug.Print("FPS: "+frames);
                    frames = 0;
                    startTime = Environment.TickCount;
                }
            }
        }

    }
}
