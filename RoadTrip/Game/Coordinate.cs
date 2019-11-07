using System;

namespace RoadTrip.Game
{
    public struct Coordinate : IEquatable<Coordinate>
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public Coordinate(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Deconstruct(out int x, out int y, out int z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        public override bool Equals(object o)
        {
            return o is Coordinate other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public bool Equals(Coordinate other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public static bool operator ==(Coordinate left, Coordinate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Coordinate left, Coordinate right)
        {
            return !left.Equals(right);
        }

        public static Coordinate operator +(Coordinate left, Coordinate right)
        {
            return new Coordinate(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public override string ToString()
        {
            return $"X:{X}, Y:{Y}, Z:{Z}";
        }
    }
}
