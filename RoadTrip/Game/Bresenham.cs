using System;
using System.Collections.Generic;

namespace RoadTrip.Game
{
    public static class Bresenham
    {
        public static IEnumerable<Coordinate> Line(Coordinate from, Coordinate to)
        {
            var (x0, y0, _) = from;
            var (x1, y1, _) = to;

            var steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep) {
                var t = x0;
                x0 = y0;
                y0 = t;
                t = x1; // swap x1 and y1
                x1 = y1;
                y1 = t;
            }

            if (x0 > x1) {
                var t = x0;
                x0 = x1;
                x1 = t;
                t = y0; // swap y0 and y1
                y0 = y1;
                y1 = t;
            }

            var dx = x1 - x0;
            var dy = Math.Abs(y1 - y0);
            var error = dx / 2;
            var ystep = y0 < y1 ? 1 : -1;
            var y = y0;
            for (var x = x0; x <= x1; x++) {
                yield return new Coordinate(steep ? y : x, steep ? x : y, 0);
                error = error - dy;
                if (error < 0) {
                    y += ystep;
                    error += dx;
                }
            }
        }
    }
}
