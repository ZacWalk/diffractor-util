#region

using System.Collections.Generic;
using System.Drawing;
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

            Bitmap tbLarge = CreateToolbar(32);
            Bitmap tbSmall = CreateToolbar(16);

            tbLarge.Save(@"out\tb_large.bmp", ImageFormat.Bmp);
            tbSmall.Save(@"out\tb_small.bmp", ImageFormat.Bmp);
        }

        private static Bitmap CreateToolbar(int xy)
        {
            var draw = new DrawIcon(xy);

            var images = new List<Image>
                             {
                                 draw.Cancel,
                                 draw.Back,
                                 draw.Next,
                                 draw.Parent,
                                 draw.Left(Color.White, draw.scale),
                                 draw.Right(Color.White, draw.scale),
                                 draw.Play,
                                 draw.Pause,
                                 draw.BackImage,
                                 draw.NextImage,
                                 new Bitmap("Icons\\Globe.png"),
                                 new Bitmap("Icons\\Pictures.png"),
                                 new Bitmap("Icons\\Print.png"),
                                 new Bitmap("Icons\\Time.png"),
                                 draw.Crop,
                                 draw.RotateLeft,
                                 draw.RotateRight,
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
                                 new Bitmap("Icons\\Sound3.png"),
                                 new Bitmap("Icons\\Audio.png"),
                                 new Bitmap("Icons\\Color.png"),
                                 new Bitmap("Icons\\Convert.png"),
                                 new Bitmap("Icons\\Copyright.png"),
                                 new Bitmap("Icons\\Edit.png"),
                                 new Bitmap("Icons\\facebook.png"),
                                 new Bitmap("Icons\\flickr.png"),
                                 new Bitmap("Icons\\Fullscreen.png"),
                                 new Bitmap("Icons\\Fullscreen2.png"),
                                 new Bitmap("Icons\\Graph.png"),
                                 new Bitmap("Icons\\Graph2.png"),
                                 new Bitmap("Icons\\Idea.png"),
                                 new Bitmap("Icons\\Import.png"),
                                 new Bitmap("Icons\\Info.png"),
                                 new Bitmap("Icons\\Mail.png"),
                                 new Bitmap("Icons\\Options.png"),
                                 new Bitmap("Icons\\Photo.png"),
                                 new Bitmap("Icons\\Repeat.png"),
                                 new Bitmap("Icons\\Resize.png"),
                                 new Bitmap("Icons\\Search.png"),
                                 new Bitmap("Icons\\Shuffle.png"),
                                 new Bitmap("Icons\\Star.png"),
                                 new Bitmap("Icons\\Tag.png"),
                                 new Bitmap("Icons\\text.png"),
                                 new Bitmap("Icons\\twitter.png"),
                                 new Bitmap("Icons\\Video.png"),
                                 new Bitmap("Icons\\Zoom.png"),
                                 new Bitmap("Icons\\color_wheel.png"),
                                 new Bitmap("Icons\\save.png"),
                                 new Bitmap("Icons\\Sort.png"),
                                 new Bitmap("Icons\\Tools.png"),
                                 new Bitmap("Icons\\Utilities.png"),
                                 new Bitmap("Icons\\Folders.png"),
                                 new Bitmap("Icons\\SmallItems.png"),
                                 new Bitmap("Icons\\Photo.png"),
                                 new Bitmap("Icons\\Help.png"),
                                 new Bitmap("Icons\\Support.png"),
                                 new Bitmap("Icons\\Fit.png"),
                                 new Bitmap("Icons\\New.png"),
                             };

            int x = 0;
            var tb = new Bitmap(xy*images.Count, xy, PixelFormat.Format32bppArgb);


            using (Graphics g = Graphics.FromImage(tb))
            using (new HighQualityRendering(g))
            {
                foreach (Image i in images)
                {
                    g.DrawImage(i, x++*xy, 0, xy, xy);
                }
            }
            return tb;
        }
    }
}