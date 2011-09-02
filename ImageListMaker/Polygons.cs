#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#endregion

namespace ImageListMaker
{
    public class Polygons : IEnumerable<PointF[]>
    {
        private static List<PointF[]> points;

        public Polygons(string xxx)
        {
            points = new List<PointF[]>();
            var polygon = new List<PointF>();

            var strings = xxx.Split(new[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < strings.Length; i++)
            {
                var isMove = strings[i] == "M";
                var isLine = strings[i] == "L";
                var isCurve = strings[i] == "C";

                if (isMove && polygon.Count > 0)
                {
                    points.Add(polygon.ToArray());
                    polygon.Clear();
                }

                if (isMove || isLine)
                {
                    var pp = new PointF(float.Parse(strings[i + 1]), float.Parse(strings[i + 2]));
                    polygon.Add(pp);
                    i += 2;
                }

                if (isCurve)
                {
                    var pp = new PointF(float.Parse(strings[i + 1]), float.Parse(strings[i + 2]));
                    polygon.Add(pp);
                    i += 6;
                }
            }

            if (polygon.Count > 0)
            {
                points.Add(polygon.ToArray());
            }
        }

        #region IEnumerable<PointF[]> Members

        IEnumerator<PointF[]> IEnumerable<PointF[]>.GetEnumerator()
        {
            return points.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return points.GetEnumerator();
        }

        #endregion

        public void Scale(RectangleF r)
        {
            var maxX = float.MinValue;
            var maxY = float.MinValue;
            var minX = float.MaxValue;
            var minY = float.MaxValue;

            foreach (var polygon in points)
            {
                foreach (var pointF in polygon)
                {
                    if (maxY < pointF.Y) maxY = pointF.Y;
                    if (maxX < pointF.X) maxX = pointF.X;
                    if (minY > pointF.Y) minY = pointF.Y;
                    if (minX > pointF.X) minX = pointF.X;
                }
            }

            var scaleX = r.Width/(maxX - minX);
            var scaleY = r.Height/(maxY - minY);

            foreach (var polygon in points)
            {
                for (var i = 0; i < polygon.Length; i++)
                {
                    var pt = polygon[i];
                    var x = (pt.X - minX)*scaleX;
                    var y = (pt.Y - minY)*scaleY;
                    polygon[i] = new PointF(x + r.X, y + r.Y);
                }
            }
        }
    }
}