using System.Drawing;

namespace ImageListMaker
{
    public class Util
    {
        public static PointF TopLeft(RectangleF rect)
        {
            return new PointF(rect.Left, rect.Top);
        }

        public static PointF TopRight(RectangleF rect)
        {
            return new PointF(rect.Right, rect.Top);
        }

        public static PointF BottomLeft(RectangleF rect)
        {
            return new PointF(rect.Left, rect.Bottom);
        }

        public static PointF BottomRight(RectangleF rect)
        {
            return new PointF(rect.Right, rect.Bottom);
        }    
    }
}