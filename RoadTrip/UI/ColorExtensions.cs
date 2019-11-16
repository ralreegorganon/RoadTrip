using System;
using System.Drawing;

namespace RoadTrip.UI
{
    public static class ColorExtensions
    {
        public static Color Greyscale(this Color color)
        {
            var luminosity = (0.21 * color.R + 0.72 * color.G + 0.07 * color.B) / 3;
            var gray = Convert.ToInt32(luminosity);
            var newColor = Color.FromArgb(255, gray, gray, gray);
            return newColor;
        }
    }
}
