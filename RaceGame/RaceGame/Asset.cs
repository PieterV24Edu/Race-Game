using System.Drawing;

namespace RaceGame
{
    public class Asset
    {
        public int assetId;
        public Bitmap imageToDisplay;
        public Point pointOfAsset;
        public int rotationOfAsset;
        public float scaleX;
        public float scaleY;
        //safe information about the car(asset)
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
