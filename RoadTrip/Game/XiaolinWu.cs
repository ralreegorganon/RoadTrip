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


    public static class Wu2
    {
        public static List<(Coordinate C, double A)> Line(Coordinate from, Coordinate to)
        {
            return Line((short) from.X, (short) from.Y, (short) to.X, (short) to.Y);
        }

        public static List<(Coordinate C, double A)> Line(short x0, short y0, short x1, short y1)
        {
            void DrawPixel(List<(Coordinate C, double A)> line, int x, int y, int a)
            {
                line.Add((new Coordinate(x, y, 0), (255 - a) / 255.0));
            }

            short NumLevels = 256;
            ushort IntensityBits = 8;

            ushort IntensityShift, ErrorAdj, ErrorAcc;
            ushort ErrorAccTemp, Weighting, WeightingComplementMask;
            short DeltaX, DeltaY, Temp, XDir;

            var pDC = new List<(Coordinate C, double A)>();

            /* Make sure the line runs top to bottom */
            if (y0 > y1) {
                Temp = y0;
                y0 = y1;
                y1 = Temp;
                Temp = x0;
                x0 = x1;
                x1 = Temp;
            }

            /* Draw the initial pixel, which is always exactly intersected by
               the line and so needs no weighting */
            DrawPixel(pDC, x0, y0, 0);

            if ((DeltaX = (short) (x1 - x0)) >= 0) {
                XDir = 1;
            }
            else {
                XDir = -1;
                DeltaX = (short) -DeltaX; /* make DeltaX positive */
            }

            /* Special-case horizontal, vertical, and diagonal lines, which
               require no weighting because they go right through the center of
               every pixel */
            if ((DeltaY = (short) (y1 - y0)) == 0) {
                /* Horizontal line */
                while (DeltaX-- != 0) {
                    x0 += XDir;
                    DrawPixel(pDC, x0, y0, 0);
                }

                return pDC;
            }

            if (DeltaX == 0) {
                /* Vertical line */
                do {
                    y0++;
                    DrawPixel(pDC, x0, y0, 0);
                } while (--DeltaY != 0);

                return pDC;
            }

            if (DeltaX == DeltaY) {
                /* Diagonal line */
                do {
                    x0 += XDir;
                    y0++;
                    DrawPixel(pDC, x0, y0, 0);
                } while (--DeltaY != 0);

                return pDC;
            }

            /* Line is not horizontal, diagonal, or vertical */
            ErrorAcc = 0; /* initialize the line error accumulator to 0 */
            /* # of bits by which to shift ErrorAcc to get intensity level */
            IntensityShift = (ushort) (16 - IntensityBits);
            /* Mask used to flip all bits in an intensity weighting, producing the
               result (1 - intensity weighting) */
            WeightingComplementMask = (ushort) (NumLevels - 1);
            /* Is this an X-major or Y-major line? */
            if (DeltaY > DeltaX) {
                /* Y-major line; calculate 16-bit fixed-point fractional part of a
                   pixel that X advances each time Y advances 1 pixel, truncating the
                   result so that we won't overrun the endpoint along the X axis */
                ErrorAdj = (ushort) (((ulong) DeltaX << 16) / (ulong) DeltaY);
                /* Draw all pixels other than the first and last */
                while (--DeltaY != 0) {
                    ErrorAccTemp = ErrorAcc; /* remember current accumulated error */
                    ErrorAcc += ErrorAdj; /* calculate error for next pixel */
                    if (ErrorAcc <= ErrorAccTemp) {
                        /* The error accumulator turned over, so advance the X coord */
                        x0 += XDir;
                    }

                    y0++; /* Y-major, so always advance Y */
                    /* The IntensityBits most significant bits of ErrorAcc give us the
                       intensity weighting for this pixel, and the complement of the
                       weighting for the paired pixel */
                    Weighting = (ushort) (ErrorAcc >> IntensityShift);
                    DrawPixel(pDC, x0, y0, 0 + Weighting);
                    DrawPixel(pDC, x0 + XDir, y0, 0 + (Weighting ^ WeightingComplementMask));
                }

                /* Draw the final pixel, which is 
                   always exactly intersected by the line
                   and so needs no weighting */
                DrawPixel(pDC, x1, y1, 0);
                return pDC;
            }

            /* It's an X-major line; calculate 16-bit fixed-point fractional part of a
               pixel that Y advances each time X advances 1 pixel, truncating the
               result to avoid overrunning the endpoint along the X axis */
            ErrorAdj = (ushort) (((ulong) DeltaY << 16) / (ulong) DeltaX);
            /* Draw all pixels other than the first and last */
            while (--DeltaX != 0) {
                ErrorAccTemp = ErrorAcc; /* remember current accumulated error */
                ErrorAcc += ErrorAdj; /* calculate error for next pixel */
                if (ErrorAcc <= ErrorAccTemp) {
                    /* The error accumulator turned over, so advance the Y coord */
                    y0++;
                }

                x0 += XDir; /* X-major, so always advance X */
                /* The IntensityBits most significant bits of ErrorAcc give us the
                   intensity weighting for this pixel, and the complement of the
                   weighting for the paired pixel */
                Weighting = (ushort) (ErrorAcc >> IntensityShift);
                DrawPixel(pDC, x0, y0, 0 + Weighting);
                DrawPixel(pDC, x0, y0 + 1, 0 + (Weighting ^ WeightingComplementMask));
            }

            /* Draw the final pixel, which is always exactly intersected by the line
               and so needs no weighting */
            DrawPixel(pDC, x1, y1, 0);

            return pDC;
        }
    }

    public static class BresenhamAA
    {
        public static List<(Coordinate C, double A)> Line(Coordinate from, Coordinate to)
        {
            return Line(from.X, from.Y, to.X, to.Y);
        }

        private static List<(Coordinate C, double A)> Line(int x0, int y0, int x1, int y1)
        {
            var path = new List<(Coordinate C, double A)>();

            void setPixelAA(List<(Coordinate C, double A)> line, int x, int y, int a)
            {
                line.Add((new Coordinate(x, y, 0), (255 - a) / 255.0));
            }

            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx - dy, e2, x2;                       /* error value e_xy */
            int ed = (int) (dx + dy == 0 ? 1 : Math.Sqrt((float)dx * dx + (float)dy * dy));

            for (; ; )
            {                                         /* pixel loop */
                setPixelAA(path, x0, y0, 255 * Math.Abs(err - dx + dy) / ed);
                e2 = err; x2 = x0;
                if (2 * e2 >= -dx)
                {                                    /* x step */
                    if (x0 == x1) break;
                    if (e2 + dy < ed) setPixelAA(path, x0, y0 + sy, (e2 + dy) / ed);
                    err -= dy; x0 += sx;
                }
                if (2 * e2 <= dy)
                {                                     /* y step */
                    if (y0 == y1) break;
                    if (dx - e2 < ed) setPixelAA(path, x2 + sx, y0, (dx - e2) / ed);
                    err += dx; y0 += sy;
                }
            }


            return path;
        }
    }
}
