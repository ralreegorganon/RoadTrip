using System;
using System.Collections.Generic;

namespace RoadTrip.Game
{
    public static class XiaolinWu
    {
        private static int ipart(double x)
        {
            return (int) x;
        }

        private static double fpart(double x)
        {
            if (x < 0) {
                return 1 - (x - Math.Floor(x));
            }

            return x - Math.Floor(x);
        }

        private static double rfpart(double x)
        {
            return 1 - fpart(x);
        }

        private static void swap(ref double item1, ref double item2)
        {
            var temp = item1;
            item1 = item2;
            item2 = temp;
        }

        public static List<(Coordinate C, float A)> Line(Coordinate from, Coordinate to)
        {
            return Line(from.X, from.Y, to.X, to.Y);
        }

        private static List<(Coordinate C, float A)> Line(double x0, double y0, double x1, double y1)
        {
            var path = new List<(Coordinate C, float A)>();

            var steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);

            if (steep) {
                swap(ref x0, ref y0);
                swap(ref x1, ref y1);
            }

            if (x0 > x1) {
                swap(ref x0, ref x1);
                swap(ref y0, ref y1);
            }

            var dx = x1 - x0;
            var dy = y1 - y0;
            var gradient = dy / dx;

            // handle first endpoint
            var xend = Math.Round(x0);
            var yend = y0 + gradient * (xend - x0);
            var xgap = rfpart(x0 + 0.5);
            var xpxl1 = xend; // this will be used in the main loop
            double ypxl1 = ipart(yend);

            if (steep) {
                path.Add((new Coordinate((int) Math.Round(ypxl1), (int) Math.Round(xpxl1), 0), Convert.ToSingle(rfpart(yend) * xgap)));
                path.Add((new Coordinate((int) Math.Round(ypxl1 + 1), (int) Math.Round(xpxl1), 0), Convert.ToSingle(fpart(yend) * xgap)));
            }
            else {
                path.Add((new Coordinate((int) Math.Round(xpxl1), (int) Math.Round(ypxl1), 0), Convert.ToSingle(rfpart(yend) * xgap)));
                path.Add((new Coordinate((int) Math.Round(xpxl1), (int) Math.Round(ypxl1 + 1), 0), Convert.ToSingle(fpart(yend) * xgap)));
            }

            // first y-intersection for the main loop
            var intery = yend + gradient;

            // handle second endpoint
            xend = Math.Round(x1);
            yend = y1 + gradient * (xend - x1);
            xgap = fpart(x1 + 0.5);
            var xpxl2 = xend; // this will be used in the main loop
            double ypxl2 = ipart(yend);

            if (steep) {
                path.Add((new Coordinate((int) Math.Round(ypxl2), (int) Math.Round(xpxl2), 0), Convert.ToSingle(rfpart(yend) * xgap)));
                path.Add((new Coordinate((int) Math.Round(ypxl2 + 1), (int) Math.Round(xpxl2), 0), Convert.ToSingle(fpart(yend) * xgap)));
            }
            else {
                path.Add((new Coordinate((int) Math.Round(xpxl2), (int) Math.Round(ypxl2), 0), Convert.ToSingle(rfpart(yend) * xgap)));
                path.Add((new Coordinate((int) Math.Round(xpxl2), (int) Math.Round(ypxl2 + 1), 0), Convert.ToSingle(fpart(yend) * xgap)));
            }

            if (steep) {
                for (var x = xpxl1 + 1; x <= xpxl2 - 1; x++) {
                    path.Add((new Coordinate(ipart(intery), (int) Math.Round(x), 0), Convert.ToSingle(rfpart(intery))));
                    path.Add((new Coordinate(ipart(intery) + 1, (int) Math.Round(x), 0), Convert.ToSingle(fpart(intery))));
                    intery = intery + gradient;
                }
            }
            else {
                for (var x = xpxl1 + 1; x <= xpxl2 - 1; x++) {
                    path.Add((new Coordinate((int) Math.Round(x), ipart(intery), 0), Convert.ToSingle(rfpart(intery))));
                    path.Add((new Coordinate((int) Math.Round(x), ipart(intery) + 1, 0), Convert.ToSingle(fpart(intery))));
                    intery = intery + gradient;
                }
            }

            return path;
        }
    }
}
