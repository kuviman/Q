using System;

namespace QE {

    [Serializable]
    public struct Vec4 {

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }

        public Vec4(double x, double y, double z, double w) : this() {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vec4(Vec3 xyz, double w) : this(xyz.X, xyz.Y, xyz.Z, w) { }

        public double this[int i] {
            get {
                switch (i) {
                case 0: return X;
                case 1: return Y;
                case 2: return Z;
                case 3: return W;
                default: throw new IndexOutOfRangeException();
                }
            }
            set {
                switch (i) {
                case 0: X = value; break;
                case 1: Y = value; break;
                case 2: Z = value; break;
                case 3: W = value; break;
                default: throw new IndexOutOfRangeException();
                }
            }
        }

        public override string ToString() {
            return string.Format("({0}; {1}; {2}; {3})", X, Y, Z, W);
        }

        public static readonly Vec4 Zero = new Vec4(0, 0, 0, 0);
        public static readonly Vec4 OrtX = new Vec4(1, 0, 0, 0);
        public static readonly Vec4 OrtY = new Vec4(0, 1, 0, 0);
        public static readonly Vec4 OrtZ = new Vec4(0, 0, 1, 0);
        public static readonly Vec4 OrtW = new Vec4(0, 0, 0, 1);

        public static Vec4 operator +(Vec4 v) {
            return new Vec4(+v.X, +v.Y, +v.Z, +v.W);
        }
        public static Vec4 operator -(Vec4 v) {
            return new Vec4(-v.X, -v.Y, -v.Z, -v.W);
        }

        public static Vec4 operator +(Vec4 a, Vec4 b) {
            return new Vec4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
        }
        public static Vec4 operator -(Vec4 a, Vec4 b) {
            return new Vec4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
        }

        public static Vec4 operator *(Vec4 v, double k) {
            return new Vec4(v.X * k, v.Y * k, v.Z * k, v.W * k);
        }
        public static Vec4 operator *(double k, Vec4 v) {
            return new Vec4(k * v.X, k * v.Y, k * v.Z, k * v.W);
        }

        public static Vec4 operator /(Vec4 v, double k) {
            return new Vec4(v.X / k, v.Y / k, v.Z / k, v.W / k);
        }

        public static double Dot(Vec4 a, Vec4 b) {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;
        }

        public double SquareLength {
            get { return Dot(this, this); }
        }

        public double Length {
            get { return Math.Sqrt(SquareLength); }
        }

        public Vec4 Unit {
            get {
                var len = Length;
                if (len < 1e-9)
                    return this;
                return this / len;
            }
        }

        public static Vec4 Clamp(Vec4 a, double maxlen) {
            if (a.Length <= maxlen)
                return a;
            else
                return a.Unit * maxlen;
        }


        public Vec2 XY {
            get { return new Vec2(X, Y); }
        }
        public Vec3 XYZ {
            get { return new Vec3(X, Y, Z); }
        }

    }

}