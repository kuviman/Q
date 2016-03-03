using System;

namespace QE {

    [Serializable]
    public struct Vec3 {
        
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        
        public Vec3(double x, double y, double z) : this() {
            X = x;
            Y = y;
            Z = z;
        }

        public Vec3(Vec2 xy, double z) : this(xy.X, xy.Y, z) { }

        public double this[int i] {
            get {
                switch (i) {
                case 0: return X;
                case 1: return Y;
                case 2: return Z;
                default: throw new IndexOutOfRangeException();
                }
            }
            set {
                switch (i) {
                case 0: X = value; break;
                case 1: Y = value; break;
                case 2: Z = value; break;
                default: throw new IndexOutOfRangeException();
                }
            }
        }

        public override string ToString() {
            return string.Format("({0}; {1}; {2})", X, Y, Z);
        }
        
        public static readonly Vec3 Zero = new Vec3(0, 0, 0);
        public static readonly Vec3 OrtX = new Vec3(1, 0, 0);
        public static readonly Vec3 OrtY = new Vec3(0, 1, 0);
        public static readonly Vec3 OrtZ = new Vec3(0, 0, 1);

        public static Vec3 operator +(Vec3 v) {
            return new Vec3(+v.X, +v.Y, +v.Z);
        }
        public static Vec3 operator -(Vec3 v) {
            return new Vec3(-v.X, -v.Y, -v.Z);
        }

        public static Vec3 operator +(Vec3 a, Vec3 b) {
            return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static Vec3 operator -(Vec3 a, Vec3 b) {
            return new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vec3 operator *(Vec3 v, double k) {
            return new Vec3(v.X * k, v.Y * k, v.Z * k);
        }
        public static Vec3 operator *(double k, Vec3 v) {
            return new Vec3(k * v.X, k * v.Y, k * v.Z);
        }

        public static Vec3 operator /(Vec3 v, double k) {
            return new Vec3(v.X / k, v.Y / k, v.Z / k);
        }
        
        public static double Dot(Vec3 a, Vec3 b) {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }
        
        public static Vec3 Cross(Vec3 a, Vec3 b) {
            return new Vec3(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }
        
        public double SquareLength {
            get { return Dot(this, this); }
        }
        
        public double Length {
            get { return Math.Sqrt(SquareLength); }
        }
        
        public Vec3 Unit {
            get {
                var len = Length;
                if (len < 1e-9)
                    return this;
                return this / len;
            }
        }

        public static Vec3 Clamp(Vec3 a, double maxlen) {
            if (a.Length <= maxlen)
                return a;
            else
                return a.Unit * maxlen;
        }


        public Vec2 XY {
            get { return new Vec2(X, Y); }
        }

    }

}