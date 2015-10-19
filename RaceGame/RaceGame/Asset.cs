using System.Drawing;

namespace RaceGame
{
    public class Asset
    {
        public int assetId;
        public Bitmap imageToDisplay;
        public Point pointOfAsset;
        public int rotationOfAsset;
        public Size scaleOfAsset;

        public Asset(int id, Bitmap img, Point pos, int rot,Size scale)
        {
            assetId = id;
            imageToDisplay = img;
            pointOfAsset = pos;
            rotationOfAsset = rot;
            scaleOfAsset = scale;
        }
    }
}
