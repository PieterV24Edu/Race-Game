using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
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
        Props
    }


    class GraphicsEngine
    {
        static object rendering = new object();

        Graphics backgroundBuffer;
        Graphics graphicsBuffer;
        Graphics drawHandle;
        Bitmap backBuffer;
        Thread mainRenderThread;
        Thread debugThread;
        static List<Asset> backgroundAssets = new List<Asset>();
        static List<Asset> playerAssets = new List<Asset>();
        static List<Asset> propAssets = new List<Asset>(); 

        public int frames;
        public int startTime;

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
            backBuffer = new Bitmap(MainWindow.width,MainWindow.height);
            graphicsBuffer = Graphics.FromImage(backBuffer);

            while (true)
            {
                if (graphicsBuffer != null)
                {
                    graphicsBuffer.Clear(Color.White);
                }

                graphicsBuffer.ResetTransform();
                BackgroundThread();
                PlayerThread();

                drawHandle.DrawImage(backBuffer,0,0);
                frames++;

            }
        }

        public void BackgroundThread()
        {

            for (int i = 0; i < backgroundAssets.Count; i++)
            {
                Bitmap tempImage = new Bitmap(backgroundAssets[i].imageToDisplay);

                float x = MainWindow.width;
                float y = MainWindow.height;

                x /= tempImage.Width+10;
                y /= tempImage.Height+10;
                
                graphicsBuffer.ScaleTransform(x,y);

                graphicsBuffer.DrawImage(tempImage, backgroundAssets[i].pointOfAsset);
                tempImage.Dispose();
            }
        }

        public void PlayerThread()
        {


            for (int i = 0; i < playerAssets.Count; i++)
            {
                Bitmap tempImage = new Bitmap(playerAssets[i].imageToDisplay);
                graphicsBuffer.DrawImage(tempImage, new Rectangle(playerAssets[i].pointOfAsset,playerAssets[i].scaleOfAsset+playerAssets[i].imageToDisplay.Size));
                tempImage.Dispose();
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
            Point tempPos = new Point(1,0);



            for (int i = 0; i < playerAssets.Count; i++)
            {
                if (playerAssets[i].assetId == assetId)
                {
                    playerAssets[i].rotationOfAsset = rot;
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
