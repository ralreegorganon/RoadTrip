using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using RoadTrip.Game;

namespace RoadTripBenchmark
{
   public class LineBenchmarks
    {
        //public List<(Coordinate, Coordinate)> Pairs { get; set; } = new List<(Coordinate, Coordinate)>();

        //[GlobalSetup]
        //public void GlobalSetup()
        //{
        //    var r = new Random();
        //    for (var i = 0; i < 10000; i++)
        //    {
        //        Pairs.Add((new Coordinate(r.Next(-4096, 4096), r.Next(-4096, 4096), 0), new Coordinate(r.Next(-4096, 4096), r.Next(-4096, 4096), 0)));
        //    }
        //}

        //[Benchmark]
        //public List<(Coordinate, float)> First()
        //{
        //    var all = new List<(Coordinate, float)>();
        //    foreach (var (from, to) in Pairs) {
        //        all.AddRange(XiaolinWu.Line(from, to));
        //    }
        //    return all;
        //}

        //[Benchmark]
        //public List<(Coordinate, double)> Second()
        //{
        //    var all = new List<(Coordinate, double)>();
        //    foreach (var (from, to) in Pairs)
        //    {
        //        all.AddRange(Wu2.Line(from, to));
        //    }
        //    return all;
        //}


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
    }
}
