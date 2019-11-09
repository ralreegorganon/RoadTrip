using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using RoadTrip.Game;

namespace RoadTripBenchmark
{
   public class LineBenchmarks
    {
        [Benchmark]
        public List<(Coordinate, float)> First()
        {
            return XiaolinWu.Line(new Coordinate(0, 0, 0), new Coordinate(10, 3, 0));
        }

        [Benchmark]
        public List<(Coordinate, double)> Second()
        {
            return Wu2.Line(new Coordinate(0,0,0), new Coordinate(10,3, 0));
        }

        [Benchmark]
        public List<(Coordinate, double)> Third()
        {
            return Wu2.Line(new Coordinate(0, 0, 0), new Coordinate(10, 3, 0));
        }
    }
}
}
