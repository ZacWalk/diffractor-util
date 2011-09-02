using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ImageListMaker
{
    public class DrawIcon
    {
        private const float inflate = 0.2f;
        private const float LineWidth = 5f;
        public const int Scale = 32;

        public static Image Back
        {
            get
            {
                var result = new Bitmap(Scale, Scale);
                using (var g = Graphics.FromImage(result))
                using (new HighQualityRendering(g))
                using (var p = CreatePen(Color.White, LineJoin.Miter, LineCap.Round, LineCap.Round, LineWidth))
                {
                    var points = new[]
                                     {
                                         new PointF(0.5f*Scale, inflate*Scale), new PointF(inflate*Scale, 0.5f*Scale),
                                         new PointF(0.5f*Scale, (1.0f - inflate)*Scale)
                                     };

                    var points2 = new[]
                                      {
                                          new PointF(inflate*Scale, 0.5f*Scale),
                                          new PointF((1.0f - inflate)*Scale, 0.5f*Scale)
                                      };

                    g.DrawLines(p, points);
                    g.DrawLines(p, points2);
                }
                return result;
            }
        }

        public static Image Next
        {
            get
            {
                var result = new Bitmap(Scale, Scale);
                using (var g = Graphics.FromImage(result))
                using (new HighQualityRendering(g))
                using (var p = CreatePen(Color.White, LineJoin.Miter, LineCap.Round, LineCap.Round, LineWidth))
                {
                    var points = new[]
                                     {
                                         new PointF(0.5f*Scale, inflate*Scale),
                                         new PointF((1.0f - inflate)*Scale, 0.5f*Scale),
                                         new PointF(0.5f*Scale, (1.0f - inflate)*Scale)
                                     };

                    var points2 = new[]
                                      {
                                          new PointF((1.0f - inflate)*Scale, 0.5f*Scale),
                                          new PointF(inflate*Scale, 0.5f*Scale)
                                      };

                    g.DrawLines(p, points);
                    g.DrawLines(p, points2);
                }
                return result;
            }
        }

        public static Image BackImage
        {
            get
            {
                var result = new Bitmap(Scale, Scale);
                using (var g = Graphics.FromImage(result))
                using (new HighQualityRendering(g))
                using (var p = CreatePen(Color.White, LineJoin.Round, LineCap.Round, LineCap.Round, LineWidth))
                {
                    g.DrawLines(p, GetLeftRightPoints(0.4f, 0.2f, 0.3f, Scale));
                    g.DrawLines(p, GetLeftRightPoints(0.8f, 0.6f, 0.3f, Scale));
                }
                return result;
            }
        }

        public static Image NextImage
        {
            get
            {
                var result = new Bitmap(Scale, Scale);
                using (var g = Graphics.FromImage(result))
                using (new HighQualityRendering(g))
                using (var p = CreatePen(Color.White, LineJoin.Round, LineCap.Round, LineCap.Round, LineWidth))
                {
                    g.DrawLines(p, GetLeftRightPoints(0.2f, 0.4f, 0.3f, Scale));
                    g.DrawLines(p, GetLeftRightPoints(0.6f, 0.8f, 0.3f, Scale));
                }
                return result;
            }
        }

        public static Image Play
        {
            get
            {
                var result = new Bitmap(Scale, Scale);
                using (var g = Graphics.FromImage(result))
                using (new HighQualityRendering(g))
                using (var p = CreatePen(Color.White, LineJoin.Round, LineCap.Round, LineCap.Round, LineWidth))
                {
                    g.DrawPolygon(p, GetLeftRightPoints(0.3f, 0.7f, 0.2f, Scale));
                }
                return result;
            }
        }

        public static Bitmap Pause
        {
            get
            {
                var result = new Bitmap(Scale, Scale);
                using (var g = Graphics.FromImage(result))
                using (new HighQualityRendering(g))
                using (var p = CreatePen(Color.White, LineJoin.Round, LineCap.Round, LineCap.Round, LineWidth))
                {
                    var xx = Scale / 3f;
                    var top = 0.2f * Scale;

                    g.DrawLine(p, xx, top, xx, Scale - top);
                    g.DrawLine(p, xx * 2f, top, xx * 2f, Scale - top);
                }
                return result;
            }
        }

        public static Image Down
        {
            get
            {
                var result = new Bitmap(Scale, Scale);
                using (var g = Graphics.FromImage(result))
                using (new HighQualityRendering(g))
                using (var p = CreatePen(Color.White, LineJoin.Round, LineCap.Round, LineCap.Round, LineWidth))
                {
                    var inflateX = 0.3f;
                    var inflateY = 0.4f;

                    var points = new[]
                                     {
                                         new PointF(inflateX*Scale, inflateY*Scale),
                                         new PointF(0.5f*Scale, (1.0f - inflateY)*Scale),
                                         new PointF((1.0f - inflateX)*Scale, inflateY*Scale)
                                     };

                    //g.FillPolygon(Brushes.White, points);
                    //g.DrawPolygon(GetPen2(), points);
                    g.DrawLines(p, points);
                }
                return result;
            }
        }

        public static Image Refresh
        {
            get
            {
                var result = new Bitmap(Scale, Scale);
                using (var g = Graphics.FromImage(result))
                using (new HighQualityRendering(g))
                using (var p = CreatePen(Color.White, LineJoin.Round, LineCap.Round, LineCap.Round, LineWidth))
                {
                    var xx = 2 + 0.5f * Scale;

                    var points = new[] { new PointF(xx - 2, 1), new PointF(xx + 4, 5), new PointF(xx - 2, 10) };

                    var r = new RectangleF(0, 0, Scale, Scale);
                    r.Inflate(-5f, -5f);
                    g.DrawArc(p, r, 0, 270);
                    g.FillPolygon(Brushes.White, points);
                }
                return result;
            }
        }

        public static Image Print
        {
            get
            {
                var result = new Bitmap(Scale, Scale);
                using (var g = Graphics.FromImage(result))
                using (new HighQualityRendering(g))
                using (var p = CreatePen(Color.White, LineJoin.Miter, LineCap.Round, LineCap.Round, 2))
                {
                    var points = new[]
                                     {
                                         new PointF(0.3f*Scale, 0.7f*Scale), new PointF(0.1f*Scale, 0.7f*Scale),
                                         new PointF(0.1f*Scale, 0.3f*Scale), new PointF(0.9f*Scale, 0.3f*Scale),
                                         new PointF(0.9f*Scale, 0.7f*Scale), new PointF(0.7f*Scale, 0.7f*Scale),
                                     };

                    var points2 = new[]
                                      {
                                          new PointF(0.3f*Scale, 0.3f*Scale), new PointF(0.3f*Scale, 0.1f*Scale),
                                          new PointF(0.7f*Scale, 0.1f*Scale), new PointF(0.7f*Scale, 0.3f*Scale),
                                      };

                    var points3 = new[]
                                      {
                                          new PointF(0.3f*Scale, 0.5f*Scale), new PointF(0.3f*Scale, 0.9f*Scale),
                                          new PointF(0.7f*Scale, 0.9f*Scale), new PointF(0.7f*Scale, 0.5f*Scale),
                                          new PointF(0.3f*Scale, 0.5f*Scale),
                                      };

                    g.DrawLines(p, points);
                    g.DrawLines(p, points2);
                    g.DrawLines(p, points3);
                }
                return result;
            }
        }

        public static Image Parent
        {
            get
            {
                var result = new Bitmap(Scale, Scale);
                using (var g = Graphics.FromImage(result))
                using (new HighQualityRendering(g))
                using (var p = CreatePen(Color.White, LineJoin.Miter, LineCap.Round, LineCap.Round, LineWidth))
                {
                    var points = new[]
                                     {
                                         new PointF(0.2f*Scale, 0.5f*Scale), new PointF(0.5f*Scale, 0.2f*Scale),
                                         new PointF(0.8f*Scale, 0.5f*Scale)
                                     };
                    var points2 = new[]
                                      {
                                          new PointF(0.5f*Scale, 0.2f*Scale), new PointF(0.5f*Scale, 0.6f*Scale),
                                          new PointF(0.3f*Scale, 0.8f*Scale)
                                      };

                    g.DrawLines(p, points);
                    g.DrawLines(p, points2);
                }
                return result;
            }
        }
        
        public static Image Cancel
        {
            get
            {
                var result = new Bitmap(Scale, Scale);
                using (var g = Graphics.FromImage(result))
                using (new HighQualityRendering(g))
                using (var p = CreatePen(Color.White, LineJoin.Miter, LineCap.Round, LineCap.Round, LineWidth))
                {
                    var points = new[]
                                     {
                                         new PointF(0.2f*Scale, 0.2f*Scale), 
                                         new PointF(0.8f*Scale, 0.8f*Scale)
                                     };
                    var points2 = new[]
                                      {
                                          new PointF(0.2f*Scale, 0.8f*Scale),
                                          new PointF(0.8f*Scale, 0.2f*Scale)
                                      };

                    g.DrawLines(p, points);
                    g.DrawLines(p, points2);
                }
                return result;
            }
        }

        

        public static Image NormalView
        {
            get
            {
                var result = new Bitmap(Scale, Scale);
                using (var g = Graphics.FromImage(result))
                using (new HighQualityRendering(g))
                using (var p25 = CreatePen(Color.White, LineJoin.Miter, LineCap.Round, LineCap.Round, 2.5f))
                {
                    var points = new[] { new PointF(0.5f * Scale, 0.35f * Scale), new PointF(0.5f * Scale, 0.65f * Scale), };

                    var r = new RectangleF(0, 0, Scale, Scale);
                    r.Inflate(-0.1f * Scale, -0.2f * Scale);
                    g.DrawRectangle(p25, Rectangle.Round(r));

                    g.DrawLines(p25, points);
                }
                return result;
            }
        }

        public static Image Time
        {
            get
            {
                var result = new Bitmap(Scale, Scale);
                using (var g = Graphics.FromImage(result))
                using (new HighQualityRendering(g))
                using (var p25 = CreatePen(Color.White, LineJoin.Miter, LineCap.Round, LineCap.Round, 2.5f))
                using (var p15 = CreatePen(Color.White, LineJoin.Miter, LineCap.Round, LineCap.Round, 1.5f))
                {
                    var points = new[]
                                     {
                                         new PointF(0.5f*Scale, 0.3f*Scale), new PointF(0.5f*Scale, 0.5f*Scale),
                                         new PointF(0.65f*Scale, 0.45f*Scale),
                                     };

                    var r = new RectangleF(0, 0, Scale, Scale);
                    r.Inflate(-5f, -5f);
                    g.DrawArc(p25, r, 0, 360);

                    g.DrawLines(p15, points);
                }
                return result;
            }
        }

        public static Image World
        {
            get
            {
                var xxx =
                    "M 911.99899 253.8101 L 917.49669 231.57396 L 951.09369 177.81883 L 933.37889 186.37078 L 898.56019 147.27614 L 904.66879 144.83272 L 936.43319 174.1537 L 965.14329 131.39394 L 956.59139 128.33967 L 948.65029 137.50248 L 936.43319 124.0637 L 941.93089 120.39857 L 958.42389 125.2854 L 978.58209 110.01406 L 992.63169 110.01406 M 884.27219 -42.699383 L 867.40669 -36.590846 L 890.00829 -9.7132796 L 866.18499 18.996846 L 858.85479 12.888308 L 867.40669 6.1689174 L 844.80509 0.060380418 L 816.09499 36.711607 L 826.47949 38.544167 L 830.14459 51.982947 L 840.52909 37.933317 L 838.69659 26.937945 L 845.41599 14.110016 L 850.91369 14.110016 L 843.58339 28.159652 L 849.08109 35.489897 L 848.47019 50.761237 L 813.04069 60.534897 L 790.43909 74.584537 L 794.71509 92.910147 L 772.72439 90.466737 L 769.05929 103.90552 L 764.78327 112.45747 L 772.11349 116.73345 L 775.77859 119.17687 L 788.60659 119.78772 L 797.76939 102.68381 L 802.65619 102.68381 L 806.32129 97.186127 L 822.81439 91.688447 L 839.30739 113.67918 L 845.41599 107.57064 L 830.14459 89.245027 L 836.25319 89.245027 L 859.46559 120.39857 L 866.18499 102.68381 L 875.34779 101.46211 L 877.79119 80.693077 L 908.33389 86.801617 L 907.72309 94.131857 L 872.90439 106.95979 L 877.18039 117.3443 L 901.61449 112.45747 L 901.00369 135.05906 L 883.89979 141.1676 L 858.24389 136.89162 L 853.96789 144.22187 L 825.25779 136.28077 L 824.64689 122.23113 L 761.729 129.56138 L 758.67473 143.00016 L 736.68399 156.43894 L 731.79716 185.75992 L 755.62046 219.96773 L 806.93219 227.90883 L 822.20349 234.01737 L 819.76009 244.40188 L 832.58799 255.39725 L 835.03149 261.50579 L 827.70119 268.83604 L 845.41599 276.16628 ";

                var polygons = new Polygons(xxx);

                var result = new Bitmap(Scale, Scale);
                using (var g = Graphics.FromImage(result))
                using (new HighQualityRendering(g))
                {
                    using (var p25 = CreatePen(Color.White, LineJoin.Miter, LineCap.Round, LineCap.Round, 2.5f))
                    using (var p15 = CreatePen(Color.White, LineJoin.Miter, LineCap.Round, LineCap.Round, 1.5f))
                    {
                        var r = new Rectangle(0, 0, Scale, Scale);
                        r.Inflate(-4, -4);

                        polygons.Scale(r);

                        foreach (PointF[] polygon in polygons)
                        {
                            g.DrawLines(p15, polygon);
                        }

                        g.DrawRectangle(p25, r);
                    }
                }
                return result;
            }
        }

        public static Image FullScreen
        {
            get
            {
                var result = new Bitmap(Scale, Scale);

                using (var g = Graphics.FromImage(result))
                using (new HighQualityRendering(g))
                using (var p25 = CreatePen(Color.White, LineJoin.Miter, LineCap.Round, LineCap.Round, 2.5f))
                using (var pArrow = CreatePen(Color.White, LineJoin.Miter, LineCap.ArrowAnchor, LineCap.ArrowAnchor, 2f))
                {
                    var dim = 0.1f * Scale;

                    var r = new RectangleF(0, 0, Scale, Scale);
                    r.Inflate(-dim, -dim * 2);
                    g.DrawRectangle(p25, Rectangle.Round(r));

                    r.Inflate(-dim * 1.5f, -dim * 1.5f);
                    g.DrawLine(pArrow, Util.BottomLeft(r), Util.TopRight(r));
                }

                return result;
            }
        }

        public static Image Resize
        {
            get
            {
                var result = new Bitmap(Scale, Scale);

                using (var g = Graphics.FromImage(result))
                using (new HighQualityRendering(g))
                using (var pArrow = CreatePen(Color.White, LineJoin.Miter, LineCap.Round, LineCap.Round, 3.3f))
                {
                    var dim = 0.2f * Scale;

                    var r = new RectangleF(0, 0, Scale, Scale);
                    r.Inflate(-dim, -dim);

                    DrawArrow(g, pArrow, Brushes.White, Util.BottomLeft(r), Util.TopLeft(r), 8);
                    DrawArrow(g, pArrow, Brushes.White, Util.BottomLeft(r), Util.BottomRight(r), 8);
                }

                return result;
            }
        }

        public static void DrawArrow(Graphics g, Pen pen, Brush brush, PointF p1, PointF p2, float size)
        {
            var tan = Math.Atan((p2.X - p1.X) / (p2.Y - p1.Y)) + (Math.PI / 2);
            var angle = Math.PI / 4;

            var head = new[] { p2, PointOnCircle(p2, tan - angle, size), PointOnCircle(p2, tan + angle, size), p2 };

            g.DrawLine(pen, p1, p2);
            g.FillPolygon(brush, head); //fill arrow head if desired
        }

        private static PointF PointOnCircle(PointF centerPoint, double angle, float distance)
        {
            return new PointF(
                (float)(centerPoint.X + distance * Math.Cos(angle)),
                (float)(centerPoint.Y + distance * Math.Sin(angle)));
        }

        public static Image Left(Color color, float scale)
        {
            var result = new Bitmap((int)scale, (int)scale);
            using (var g = Graphics.FromImage(result))
            using (new HighQualityRendering(g))
            using (var p = CreatePen(color, LineJoin.Round, LineCap.Round, LineCap.Round, scale / 6))
            {
                g.DrawLines(p, GetLeftRightPoints(0.6f, 0.4f, 0.3f, scale));
            }
            return result;
        }

        public static Image Right(Color color, float scale)
        {
            var result = new Bitmap((int)scale, (int)scale);
            using (var g = Graphics.FromImage(result))
            using (new HighQualityRendering(g))
            {
                var cap = LineCap.Round;
                using (var p = CreatePen(color, LineJoin.Round, cap, cap, scale / 6))
                {
                    g.DrawLines(p, GetLeftRightPoints(0.4f, 0.6f, 0.3f, scale));
                }
            }
            return result;
        }

        private static PointF[] GetLeftRightPoints(float x1, float x2, float y, float scale)
        {
            return new[]
                       {
                           new PointF(x1*scale, y*scale), new PointF(x2*scale, 0.5f*scale),
                           new PointF(x1*scale, (1.0f - y)*scale)
                       };
        }

        public static Pen CreatePen(Color color, LineJoin miter, LineCap startCap, LineCap endCap, float width)
        {
            var p = new Pen(color, width);
            p.LineJoin = miter;
            p.StartCap = startCap;
            p.EndCap = endCap;
            return p;
        }

        public static Bitmap Add(Color color, float scale)
        {
            var result = new Bitmap((int)scale, (int)scale);
            using (var g = Graphics.FromImage(result))
            using (new HighQualityRendering(g))
            using (var p = CreatePen(color, LineJoin.Round, LineCap.Round, LineCap.Round, scale / 7f))
            {
                var padding = scale / 7f;

                g.DrawLine(p, scale / 2f, padding, scale / 2f, scale - padding);
                g.DrawLine(p, padding, scale / 2f, scale - padding, scale / 2f);
            }
            return result;
        }

        public static Bitmap Delete(Color color, float scale)
        {
            var result = new Bitmap((int)scale, (int)scale);
            using (var g = Graphics.FromImage(result))
            using (new HighQualityRendering(g))
            using (var p = CreatePen(color, LineJoin.Round, LineCap.Round, LineCap.Round, scale / 7f))
            {
                var padding = scale / 4f;

                g.DrawLine(p, padding, padding, scale - padding, scale - padding);
                g.DrawLine(p, scale - padding, padding, padding, scale - padding);
            }
            return result;
        }

        public static Bitmap PhotoSet(Color color, float scale)
        {
            var result = new Bitmap((int)scale, (int)scale);
            using (var g = Graphics.FromImage(result))
            using (new HighQualityRendering(g))
            using (var p = CreatePen(color, LineJoin.Round, LineCap.Round, LineCap.Round, scale / 16f))
            {
                var pading = (scale / 7f);
                var width = (scale / 2f) - pading;

                for (var i = 0; i < 4; i++)
                {
                    var x = pading * (i + 1f);

                    if (i == 0)
                    {
                        g.DrawRectangle(p, x, x, width, width);
                    }
                    else
                    {
                        g.DrawLines(p, new[] { new PointF(x, x + width), new PointF(x + width, x + width), new PointF(x + width, x) });
                    }
                }
            }
            return result;
        }

        public static Image Crop
        {
            get
            {
                var result = new Bitmap(Scale, Scale);
                using (var g = Graphics.FromImage(result))
                using (new HighQualityRendering(g))
                using (var p = CreatePen(Color.White, LineJoin.Miter, LineCap.Round, LineCap.Round, 4))
                {
                    var points = new[]
                                     {
                                         new PointF(0.8f*Scale, 0.3f*Scale),
                                         new PointF(0.3f*Scale, 0.3f*Scale), 
                                         new PointF(0.3f*Scale, 0.8f*Scale)
                                     };
                    var points2 = new[]
                                      {
                                          new PointF(0.2f*Scale, 0.7f*Scale),
                                          new PointF(0.7f*Scale, 0.7f*Scale),
                                          new PointF(0.7f*Scale, 0.2f*Scale)
                                      };

                    g.DrawLines(p, points);
                    g.DrawLines(p, points2);
                }
                return result;
            }
        }

        public static Image RotateLeft
        {
            get
            {
                var result = new Bitmap(Scale, Scale);
                using (var g = Graphics.FromImage(result))
                using (new HighQualityRendering(g))
                using (var p = CreatePen(Color.White, LineJoin.Miter, LineCap.Round, LineCap.Round, 4))
                {
                    var points = new[]
                                     {
                                         new PointF(inflate*Scale, 0.5f*Scale),
                                         new PointF(0.5f*Scale, (1.0f - inflate)*Scale),
                                         new PointF((1.0f - inflate)*Scale, 0.5f*Scale)
                                     };

                    var points2 = new[]
                                      {
                                          new PointF(0.5f*Scale, (1.0f - inflate)*Scale),
                                          new PointF(0.5f*Scale, inflate*Scale),
                                          new PointF((1.0f - inflate)*Scale, inflate*Scale)
                                      };

                    g.DrawLines(p, points);
                    g.DrawLines(p, points2);
                }
                return result;
            }
        }

        public static Image RotateRight
        {
            get
            {
                var result = new Bitmap(Scale, Scale);
                using (var g = Graphics.FromImage(result))
                using (new HighQualityRendering(g))
                using (var p = CreatePen(Color.White, LineJoin.Miter, LineCap.Round, LineCap.Round, 4))
                {
                    var points = new[]
                                     {
                                         new PointF(inflate*Scale, 0.5f*Scale),
                                         new PointF(0.5f*Scale, (1.0f - inflate)*Scale),
                                         new PointF((1.0f - inflate)*Scale, 0.5f*Scale)
                                     };

                    var points2 = new[]
                                      {
                                          new PointF(0.5f*Scale, (1.0f - inflate)*Scale),
                                          new PointF(0.5f*Scale, inflate*Scale),
                                          new PointF(inflate*Scale, inflate*Scale)
                                      };

                    g.DrawLines(p, points);
                    g.DrawLines(p, points2);
                }
                return result;
            }
        }
    }


}