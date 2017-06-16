using System;

namespace EW
{
    public struct WPos:IEquatable<WPos>
    {
        public readonly int X, Y, Z;

        public WPos(int x,int y,int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public WPos(WDist x,WDist y,WDist z)
        {
            X = x.Length;
            Y = y.Length;
            Z = z.Length;
           
        }
        public bool Equals(WPos other)
        {
            return other == this;
        }

        #region operator

        public static bool operator ==(WPos a,WPos b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(WPos a,WPos b)
        {
            return !(a == b);
        }

        public static WPos operator +(WPos a,WVect b)
        {
            return new WPos(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static WPos operator -(WPos a,WVect b)
        {
            return new WPos(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static  WVect operator -(WPos a,WPos b)
        {
            return new WVect(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
        #endregion
    }
}