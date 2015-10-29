using System.Drawing;

namespace RaceGame
{
    /// <summary>
    /// Used for storing information on an asset to render with GraphicsEngine
    /// </summary>
    public class Asset
    {
        /// <summary>
        /// The id of the asset
        /// </summary>
        public int assetId;

        /// <summary>
        /// The image of the asset
        /// </summary>
        public Bitmap imageToDisplay;

        /// <summary>
        /// The point on the screen 
        /// </summary>
        public Point pointOfAsset;

        /// <summary>
        /// The rotation of the asset
        /// </summary>
        public int rotationOfAsset;
        
        /// <summary>
        /// The x scale of the asset
        /// </summary>
        public float scaleX;

        /// <summary>
        /// The y scale of the asset
        /// </summary>
        public float scaleY;

        //Safe information about the asset
        public Asset(int id, Bitmap img, Point pos, int rot,float xScale,float yScale)
        {
            assetId = id;
            imageToDisplay = img;
            pointOfAsset = pos;
            rotationOfAsset = rot;
            scaleX = xScale;
            scaleY = yScale;
        }
    }
}
