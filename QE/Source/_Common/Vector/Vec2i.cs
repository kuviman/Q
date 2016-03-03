using System;

namespace QE {

    [Serializable]
    public struct Vec2i {
        
        public int X { get; set; }
        public int Y { get; set; }

        public Vec2i(int x, int y) : this() {
            X = x;
            Y = y;
        }

        public override string ToString() {
            return string.Format("({0}; {1})", X, Y);
        }

        public static readonly Vec2i Zero = new Vec2i(0, 0);
        public static readonly Vec2i OrtX = new Vec2i(1, 0);
        public static readonly Vec2i OrtY = new Vec2i(0, 1);

        public static Vec2i operator +(Vec2i v) {
            return new Vec2i(+v.X, +v.Y);
        }
        public static Vec2i operator -(Vec2i v) {
            return new Vec2i(-v.X, -v.Y);
        }

        public static Vec2i operator +(Vec2i a, Vec2i b) {
            return new Vec2i(a.X + b.X, a.Y + b.Y);
        }
        public static Vec2i operator -(Vec2i a, Vec2i b) {
            return new Vec2i(a.X - b.X, a.Y - b.Y);
        }

        public static Vec2i operator *(Vec2i v, int k) {
            return new Vec2i(v.X * k, v.Y * k);
        }
        public static Vec2i operator *(int k, Vec2i v) {
            return new Vec2i(k * v.X, k * v.Y);
        }
        
        public static int Dot(Vec2i a, Vec2i b) {
            return a.X * b.X + a.Y * b.Y;
        }
        
        public static int Skew(Vec2i a, Vec2i b) {
            return a.X * b.Y - a.Y * b.X;
        }
        
        public static Vec2i CompMult(Vec2i a, Vec2i b) {
            return new Vec2i(a.X * b.X, a.Y * b.Y);
        }

        public int SquareLength {
            get { return Dot(this, this); }
        }

    }

}