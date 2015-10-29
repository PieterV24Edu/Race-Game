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
    /// <summary>
    /// Enum used to add assets to the right list
    /// </summary>
    enum RenderType
    {
        Player,
        Props
    }

    /// <summary>
    /// Class used to draw objects on screen
    /// </summary>
    class GraphicsEngine
    {
        /// <summary>
        /// Current amount of assets to render
        /// </summary>
        public static int assetsToRender = 0;

        /// <summary>
        /// Holds frames per second
        /// </summary>
        public int frames;
        /// <summary>
        /// The time when the game started
        /// </summary>
        public int startTime;

        /// <summary>
        /// The Graphics buffer used for drawing on backBuffer
        /// </summary>
        Graphics graphicsBuffer;
        
        /// <summary>
        /// Instance of a Control on the screen
        /// </summary>
        public Graphics drawHandle;

        /// <summary>
        /// The buffer used to draw on to
        /// </summary>
        public Bitmap backBuffer;
        
        /// <summary>
        /// Assets to draw on top of the background
        /// </summary>
        static List<Asset> playerAssets = new List<Asset>();

        /// <summary>
        /// Assets to draw on top of the players
        /// </summary>
        static List<Asset> propAssets = new List<Asset>();
        
        /// <summary>
        /// Used to save perfomance during transformation of the player assets
        /// </summary>
        Point temporaryPlayerPoint;

        /// <summary>
        /// Initializes GraphicsEngine with a Graphics from a Control
        /// </summary>
        /// <param name="dHandle">Created from a Control</param>
        public GraphicsEngine(Graphics dHandle)
        {
            drawHandle = dHandle;
        }

        /// <summary>
        /// Main render function
        /// </summary>
        public void GraphicsUpdate()
        {

            //Create a black bitmap to draw upon
            backBuffer = new Bitmap(MainWindow.screenSize.Width, MainWindow.screenSize.Height);

            //Create a Graphics to draw on the blank bitmap
            graphicsBuffer = Graphics.FromImage(backBuffer);

            //Reset all transformations to make sure the background gets drawn from 0,0
            graphicsBuffer.ResetTransform();

            if (graphicsBuffer != null)
            {
                graphicsBuffer.Clear(Color.Green);                    
            }
                
            //Create a scaled version of the background picture
            Bitmap temp = new Bitmap(Resources.Background, MainWindow.screenSize.Width, MainWindow.screenSize.Height);

            //Draw the background to the buffer image
            graphicsBuffer.DrawImage(temp, new PointF(0, 0));
                
            //Dispose of the background bitmap
            temp.Dispose();

            //Start rendering the player assets
            PlayerThread();
                
            //Draw the new bitmap to the screen
            drawHandle.DrawImage(backBuffer, 0,0);
                
            frames++;
        }

        /// <summary>
        /// Function used to translate and rotate the playerAssets
        /// </summary>
        public void PlayerThread()
        {
                for (int i = 0; i < playerAssets.Count; i++)
                {
                    Matrix rotate = new Matrix();

                    //Set the current point to the middle of the image
                    temporaryPlayerPoint.X = Convert.ToInt32(playerAssets[i].pointOfAsset.X + (playerAssets[i].imageToDisplay.Width / 2));
                    temporaryPlayerPoint.Y = Convert.ToInt32(playerAssets[i].pointOfAsset.Y + (playerAssets[i].imageToDisplay.Height / 2));

                    //Scale the matrix
                    rotate.Scale(playerAssets[i].scaleX,playerAssets[i].scaleY);
                    //Rotate the matrix
                    rotate.RotateAt(playerAssets[i].rotationOfAsset,temporaryPlayerPoint);
                    
                    //Asign the scaled and rotated matrix to the buffer
                    graphicsBuffer.Transform = rotate;

                    //Draw the asset to the backbuffer
                    graphicsBuffer.DrawImage(playerAssets[i].imageToDisplay, playerAssets[i].pointOfAsset);
                }

        }

        /// <summary>
        /// Used to add an asset to render on the screen
        /// </summary>
        /// <param name="assetToRender">The asset to render</param>
        /// <param name="type">The type of the asset (RenderType.Player , RenderType.Props</param>
        public static void AddAsset(Asset assetToRender, RenderType type)
        {
                switch (type)
                {
                        case RenderType.Player:
                            //add on top of players
                            playerAssets.Add(assetToRender);
                            break;
                        case RenderType.Props:
                            //add on top of props
                            propAssets.Add(assetToRender);
                            break;
                }
        }

        /// <summary>
        /// Update the position of an asset 
        /// </summary>
        /// <param name="assetId">The asset to update</param>
        /// <param name="pos">The new position</param>
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

        /// <summary>
        /// Update the rotation of an asset
        /// </summary>
        /// <param name="assetId">The asset to update</param>
        /// <param name="rot">The new rotation</param>
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
        
        /// <summary>
        /// Update the scale of an asset
        /// </summary>
        /// <param name="assetId">The asset to update</param>
        /// <param name="scaleX">The new x scale</param>
        /// <param name="scaleY">The new y scale</param>
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
