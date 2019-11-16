using System.Collections.Generic;

namespace RoadTrip.Game
{
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

    public static class Wu2Pair
    {
        public static List<((Coordinate C, double A) First, (Coordinate C, double A) Second)> Line(Coordinate from, Coordinate to)
        {
            return Line((short)from.X, (short)from.Y, (short)to.X, (short)to.Y);
        }

        public static List<((Coordinate C, double A) First, (Coordinate C, double A) Second)> Line(short x0, short y0, short x1, short y1)
        {
            void DrawPixel(List<((Coordinate C, double A) First, (Coordinate C, double A) Second)> line, (int x, int y, int a) first, (int x, int y, int a) second)
            {
                var a = (new Coordinate(first.x, first.y, 0), (255 - first.a) / 255.0);
                var b = (new Coordinate(second.x, second.y, 0), (255 - second.a) / 255.0);
                line.Add((a,b));
            }

            short NumLevels = 256;
            ushort IntensityBits = 8;

            ushort IntensityShift, ErrorAdj, ErrorAcc;
            ushort ErrorAccTemp, Weighting, WeightingComplementMask;
            short DeltaX, DeltaY, Temp, XDir;

            var pDC = new List<((Coordinate C, double A) First, (Coordinate C, double A) Second)>();

            /* Make sure the line runs top to bottom */
            if (y0 > y1)
            {
                Temp = y0;
                y0 = y1;
                y1 = Temp;
                Temp = x0;
                x0 = x1;
                x1 = Temp;
            }

            /* Draw the initial pixel, which is always exactly intersected by
               the line and so needs no weighting */
            DrawPixel(pDC, (x0, y0, 0), (x0, y0, 0));

            if ((DeltaX = (short)(x1 - x0)) >= 0)
            {
                XDir = 1;
            }
            else
            {
                XDir = -1;
                DeltaX = (short)-DeltaX; /* make DeltaX positive */
            }

            /* Special-case horizontal, vertical, and diagonal lines, which
               require no weighting because they go right through the center of
               every pixel */
            if ((DeltaY = (short)(y1 - y0)) == 0)
            {
                /* Horizontal line */
                while (DeltaX-- != 0)
                {
                    x0 += XDir;
                    DrawPixel(pDC, (x0, y0, 0), (x0, y0, 0));
                }

                return pDC;
            }

            if (DeltaX == 0)
            {
                /* Vertical line */
                do
                {
                    y0++;
                    DrawPixel(pDC, (x0, y0, 0), (x0, y0, 0));
                } while (--DeltaY != 0);

                return pDC;
            }

            if (DeltaX == DeltaY)
            {
                /* Diagonal line */
                do
                {
                    x0 += XDir;
                    y0++;
                    DrawPixel(pDC, (x0, y0, 0), (x0, y0, 0));
                } while (--DeltaY != 0);

                return pDC;
            }

            /* Line is not horizontal, diagonal, or vertical */
            ErrorAcc = 0; /* initialize the line error accumulator to 0 */
            /* # of bits by which to shift ErrorAcc to get intensity level */
            IntensityShift = (ushort)(16 - IntensityBits);
            /* Mask used to flip all bits in an intensity weighting, producing the
               result (1 - intensity weighting) */
            WeightingComplementMask = (ushort)(NumLevels - 1);
            /* Is this an X-major or Y-major line? */
            if (DeltaY > DeltaX)
            {
                /* Y-major line; calculate 16-bit fixed-point fractional part of a
                   pixel that X advances each time Y advances 1 pixel, truncating the
                   result so that we won't overrun the endpoint along the X axis */
                ErrorAdj = (ushort)(((ulong)DeltaX << 16) / (ulong)DeltaY);
                /* Draw all pixels other than the first and last */
                while (--DeltaY != 0)
                {
                    ErrorAccTemp = ErrorAcc; /* remember current accumulated error */
                    ErrorAcc += ErrorAdj; /* calculate error for next pixel */
                    if (ErrorAcc <= ErrorAccTemp)
                    {
                        /* The error accumulator turned over, so advance the X coord */
                        x0 += XDir;
                    }

                    y0++; /* Y-major, so always advance Y */
                    /* The IntensityBits most significant bits of ErrorAcc give us the
                       intensity weighting for this pixel, and the complement of the
                       weighting for the paired pixel */
                    Weighting = (ushort)(ErrorAcc >> IntensityShift);
                    DrawPixel(pDC, (x0, y0, 0 + Weighting), (x0 + XDir, y0, 0 + (Weighting ^ WeightingComplementMask)));
                }

                /* Draw the final pixel, which is 
                   always exactly intersected by the line
                   and so needs no weighting */
                DrawPixel(pDC, (x1, y1, 0), (x1, y1, 0));
                return pDC;
            }

            /* It's an X-major line; calculate 16-bit fixed-point fractional part of a
               pixel that Y advances each time X advances 1 pixel, truncating the
               result to avoid overrunning the endpoint along the X axis */
            ErrorAdj = (ushort)(((ulong)DeltaY << 16) / (ulong)DeltaX);
            /* Draw all pixels other than the first and last */
            while (--DeltaX != 0)
            {
                ErrorAccTemp = ErrorAcc; /* remember current accumulated error */
                ErrorAcc += ErrorAdj; /* calculate error for next pixel */
                if (ErrorAcc <= ErrorAccTemp)
                {
                    /* The error accumulator turned over, so advance the Y coord */
                    y0++;
                }

                x0 += XDir; /* X-major, so always advance X */
                /* The IntensityBits most significant bits of ErrorAcc give us the
                   intensity weighting for this pixel, and the complement of the
                   weighting for the paired pixel */
                Weighting = (ushort)(ErrorAcc >> IntensityShift);
                DrawPixel(pDC, (x0, y0, 0 + Weighting),(x0, y0 + 1, 0 + (Weighting ^ WeightingComplementMask)));
            }

            /* Draw the final pixel, which is always exactly intersected by the line
               and so needs no weighting */
            DrawPixel(pDC, (x1, y1, 0), (x1, y1, 0));

            return pDC;
        }
    }

}
