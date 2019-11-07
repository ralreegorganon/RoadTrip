using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System;


namespace RoadTripBenchmark
{
    public struct CoordinateA : IEquatable<CoordinateA>
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public CoordinateA(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override bool Equals(object o)
        {
            return o is CoordinateA other && Equals(other);
        }

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public bool Equals(CoordinateA other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }
    }

    public struct CoordinateB : IEquatable<CoordinateB>
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public CoordinateB(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override bool Equals(object o)
        {
            return o is CoordinateB other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                const ulong a = 2862933555777941757;
                var hashCode = (ulong) Z;
                hashCode *= a;
                hashCode += (ulong) Y;
                hashCode *= a;
                hashCode += (ulong) X;
                return (int) hashCode;
            }
        }

        public bool Equals(CoordinateB other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }
    }

    public class CoordinateHashBenchmark
    {
        public HashSet<CoordinateA> HashSetA { get; set; } = new HashSet<CoordinateA>();
        public HashSet<CoordinateB> HashSetB { get; set; } = new HashSet<CoordinateB>();
        public Dictionary<CoordinateA, bool> DictionaryA { get; set; } = new Dictionary<CoordinateA, bool>();
        public Dictionary<CoordinateB, bool> DictionaryB { get; set; } = new Dictionary<CoordinateB, bool>();
        public List<CoordinateA> LookupA { get; set; } = new List<CoordinateA>();
        public List<CoordinateB> LookupB { get; set; } = new List<CoordinateB>();

        [GlobalSetup]
        public void GlobalSetup()
        {
            var r = new Random();
            for(var x = 0; x < 4096; x++) {
                for(var y = 0; y < 4096; y++) {
                    HashSetA.Add(new CoordinateA(x, y, 0));
                    HashSetB.Add(new CoordinateB(x, y, 0));
                    DictionaryA[new CoordinateA(x, y, 0)] = true;
                    DictionaryB[new CoordinateB(x, y, 0)] = true;
                }
            }

            for(var i = 0; i < 10000; i++) {
                var x = r.Next(0, 4096);
                var y = r.Next(0, 4096);
                LookupA.Add(new CoordinateA(x, y, 0));
                LookupB.Add(new CoordinateB(x, y, 0));
            }
        }

        [Benchmark]
        public int HashSetOptionA()
        {
            var hits = 0;
            foreach(var c in LookupA) {
                if(HashSetA.Contains(c)) {
                    hits++;
                }
            }
            return hits;
        }

        [Benchmark]
        public int HashSetOptionB()
        {
            var hits = 0;
            foreach(var c in LookupB) {
                if(HashSetB.Contains(c)) {
                    hits++;
                }
            }
            return hits;
        }

        [Benchmark]
        public int DictionaryOptionA()
        {
            var hits = 0;
            foreach(var c in LookupA) {
                if(DictionaryA.TryGetValue(c, out var it)) {
                    if(it) {
                        hits++;
                    }
                }
            }
            return hits;
        }

        [Benchmark]
        public int DictionaryOptionB()
        {
            var hits = 0;
            foreach(var c in LookupB) {
                if(DictionaryB.TryGetValue(c, out var it)) {
                    if(it) {
                        hits++;
                    }
                }
            }
            return hits;
        }
    }
}