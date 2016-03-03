using System;

namespace QE {

    [Serializable]
    public struct Vec2 {
        
        public double X { get; set; }
        public double Y { get; set; }
        
        public Vec2(double x, double y) : this() {
            X = x;
            Y = y;
        }
        
        public override string ToString() {
            return string.Format("({0}; {1})", X, Y);
        }

        public static readonly Vec2 Zero = new Vec2(0, 0);
        public static readonly Vec2 OrtX = new Vec2(1, 0);
        public static readonly Vec2 OrtY = new Vec2(0, 1);

        public static Vec2 operator +(Vec2 v) {
            return new Vec2(+v.X, +v.Y);
        }
        public static Vec2 operator -(Vec2 v) {
            return new Vec2(-v.X, -v.Y);
        }

        public static Vec2 operator +(Vec2 a, Vec2 b) {
            return new Vec2(a.X + b.X, a.Y + b.Y);
        }
        public static Vec2 operator -(Vec2 a, Vec2 b) {
            return new Vec2(a.X - b.X, a.Y - b.Y);
        }

        public static Vec2 operator *(Vec2 v, double k) {
            return new Vec2(v.X * k, v.Y * k);
        }
        public static Vec2 operator *(double k, Vec2 v) {
            return new Vec2(k * v.X, k * v.Y);
        }

        public static Vec2 operator /(Vec2 v, double k) {
            return new Vec2(v.X / k, v.Y / k);
        }
        
        public static double Dot(Vec2 a, Vec2 b) {
            return a.X * b.X + a.Y * b.Y;
        }
        
        public static double Skew(Vec2 a, Vec2 b) {
            return a.X * b.Y - a.Y * b.X;
        }
        
        public static Vec2 CompMult(Vec2 a, Vec2 b) {
            return new Vec2(a.X * b.X, a.Y * b.Y);
        }
        
        public static Vec2 CompDiv(Vec2 a, Vec2 b) {
            return new Vec2(a.X / b.X, a.Y / b.Y);
        }
        
        public double SquareLength {
            get { return Dot(this, this); }
        }
        
        public double Length {
            get { return Math.Sqrt(SquareLength); }
        }
        
        public Vec2 Unit {
            get {
                var len = Length;
                if (len < 1e-9)
                    return this;
                return this / len;
            }
        }

        public double Arg {
            get { return Math.Atan2(Y, X); }
        }
        
        public static Vec2 Rotate(Vec2 a, double angle) {
            double s = Math.Sin(angle);
            double c = Math.Cos(angle);
            return new Vec2(a.X * c - a.Y * s, a.X * s + a.Y * c);
        }
        
        public static Vec2 Rotate90(Vec2 a) {
            return new Vec2(-a.Y, a.X);
        }
        
        public static Vec2 Clamp(Vec2 a, double maxlen) {
            if (a.Length <= maxlen)
                return a;
            else
                return a.Unit * maxlen;
        }

        public static explicit operator Vec2i(Vec2 v) {
            return new Vec2i((int)v.X, (int)v.Y);
        }
        public static implicit operator Vec2(Vec2i v) {
            return new Vec2(v.X, v.Y);
        }

    }

}