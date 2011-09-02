#region

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

#endregion

namespace ImageListMaker
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Directory.CreateDirectory("out");

            var skin = new ControlSkin();
            skin.Save();

            var tbLarge = CreateLargeToolbar();
            var tbSmall = CreateSmallToolbar(tbLarge);

            tbLarge.Save(@"out\tb_large.bmp", ImageFormat.Bmp);
            tbSmall.Save(@"out\tb_small.bmp", ImageFormat.Bmp);
        }

        private static Bitmap CreateSmallToolbar(Bitmap tb)
        {
            var cx = tb.Width / 2;
            var cy = tb.Height / 2;
            var tbSmall = new Bitmap(cx, cy, PixelFormat.Format32bppArgb);

            using (var g = Graphics.FromImage(tbSmall))
            using (new HighQualityRendering(g))
            {
                g.DrawImage(tb, 0, 0, cx, cy);
            }
            return tbSmall;
        }

        private static Bitmap CreateLargeToolbar()
        {
            var images = new List<Image>
                             {
                                 DrawIcon.Cancel,
                                 DrawIcon.Back,
                                 DrawIcon.Next,
                                 DrawIcon.Parent,
                                 DrawIcon.Left(Color.White, DrawIcon.Scale),
                                 DrawIcon.Right(Color.White, DrawIcon.Scale),
                                 DrawIcon.Play,
                                 DrawIcon.Pause,
                                 DrawIcon.BackImage,
                                 DrawIcon.NextImage,
                                 new Bitmap("Icons\\Globe.png"),
                                 new Bitmap("Icons\\Pictures.png"),
                                 new Bitmap("Icons\\Print.png"),
                                 new Bitmap("Icons\\Time.png"),
                                 DrawIcon.Crop,
                                 DrawIcon.RotateLeft,
                                 DrawIcon.RotateRight,
                                 new Bitmap("Icons\\Dashboard.png"),
                                 new Bitmap("Icons\\Cancel.png"),
                                 new Bitmap("Icons\\Check.png"),
                                 new Bitmap("Icons\\Reset.png"),
                                 new Bitmap("Icons\\Display.png"),
                                 new Bitmap("Icons\\SmallItems.png"),
                                 new Bitmap("Icons\\LargeItems.png"),
                                 new Bitmap("Icons\\DetailedItems.png"),
                                 new Bitmap("Icons\\Home.png"),
                                 new Bitmap("Icons\\Play.png"),
                                 new Bitmap("Icons\\Pause.png"),
                                 new Bitmap("Icons\\Sound0.png"),
                                 new Bitmap("Icons\\Sound1.png"),
                                 new Bitmap("Icons\\Sound2.png"),
                                 new Bitmap("Icons\\Sound3.png")
                             };

            var x = 0;
            var tb = new Bitmap(DrawIcon.Scale * images.Count, DrawIcon.Scale, PixelFormat.Format32bppArgb);

            using (var g = Graphics.FromImage(tb))
            using (new HighQualityRendering(g))
            {
                foreach (var i in images)
                {
                    g.DrawImage(i, x++*DrawIcon.Scale, 0, DrawIcon.Scale, DrawIcon.Scale);
                }
            }
            return tb;
        }
    }

    public class ControlSkin
    {
        private readonly Bitmap bitmap = new Bitmap(64, 64, PixelFormat.Format32bppPArgb);

        public void RoundRectangle(Graphics g, Brush b, float h, float v, float width, float height, float radius)
        {
            using (var gp = new GraphicsPath())
            {
                gp.AddLine(h + radius, v, h + width - (radius*2), v);
                gp.AddArc(h + width - (radius*2), v, radius*2, radius*2, 270, 90);
                gp.AddLine(h + width, v + radius, h + width, v + height - (radius*2));
                gp.AddArc(h + width - (radius*2), v + height - (radius*2), radius*2, radius*2, 0, 90); // Corner
                gp.AddLine(h + width - (radius*2), v + height, h + radius, v + height);
                gp.AddArc(h, v + height - (radius*2), radius*2, radius*2, 90, 90);
                gp.AddLine(h, v + height - (radius*2), h, v + radius);
                gp.AddArc(h, v, radius*2, radius*2, 180, 90);
                gp.CloseFigure();

                g.FillPath(b, gp);
            }
        }

        public void Save()
        {
            using (var g = Graphics.FromImage(bitmap))
            using (new HighQualityRendering(g))
            {
                //g.Clear(Color.FromArgb(0x33, 0x38, 0x40));
                RoundRectangle(g, Brushes.Black, 1, 1, 29, 29, 6);

                DrawScrollBars(g, Color.FromArgb(0x80, Color.White), 32);
                DrawScrollBars(g, Color.FromArgb(0xD0, 0x11, 0x66, 0xCC), 48);
            }

            bitmap.Save(@"out\Controls.bmp", ImageFormat.Bmp);
        }

        private void DrawScrollBars(Graphics g, Color c, int x)
        {
            using (var p = DrawIcon.CreatePen(c, LineJoin.Miter, LineCap.Round, LineCap.Round, 4))
            {
                var pointsU = new[] {new PointF(x + 4, 12), new PointF(x + 8, 4), new PointF(x + 12, 12)};
                var pointsD = new[] {new PointF(x + 4, 64 - 12), new PointF(x + 8, 64 - 4), new PointF(x + 12, 64 - 12)};

                g.DrawLines(p, pointsU);
                g.DrawLines(p, pointsD);
            }

            using (var p = DrawIcon.CreatePen(c, LineJoin.Miter, LineCap.Round, LineCap.Round, 8))
            {
                var points = new[] {new PointF(x + 8, 24), new PointF(x + 8, 64 - 24)};
                g.DrawLines(p, points);
            }
        }
    }
}