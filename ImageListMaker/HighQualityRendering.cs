#region

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

#endregion

namespace ImageListMaker
{
    public class HighQualityRendering : IDisposable
    {
        private readonly CompositingQuality compositingQuality;
        private readonly Graphics g;
        private readonly InterpolationMode interpolationMode;
        private readonly SmoothingMode smoothingMode;
        private readonly TextRenderingHint textRenderingHint;

        public HighQualityRendering(Graphics g)
        {
            this.g = g;
            interpolationMode = g.InterpolationMode;
            compositingQuality = g.CompositingQuality;
            smoothingMode = g.SmoothingMode;
            textRenderingHint = g.TextRenderingHint;

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        }

        #region IDisposable Members

        public void Dispose()
        {
            g.InterpolationMode = interpolationMode;
            g.CompositingQuality = compositingQuality;
            g.SmoothingMode = smoothingMode;
            g.TextRenderingHint = textRenderingHint;
        }

        #endregion
    }
}