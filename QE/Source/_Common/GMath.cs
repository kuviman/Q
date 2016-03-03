using System;

namespace QE {

    public static class GMath {
        
        public static int Sqr(int x) {
            return x * x;
        }
        
        public static long Sqr(long x) {
            return x * x;
        }
        
        public static double Sqr(double x) {
            return x * x;
        }
        
        public static double Frac(double x) {
            return x - Math.Floor(x);
        }
        
        public static int Floor(double x) {
            return (int)Math.Floor(x);
        }
        
        public static Vec2i Floor(Vec2 v) {
            return new Vec2i(Floor(v.X), Floor(v.Y));
        }
        
        public static int Ceil(double x) {
            return (int)Math.Ceiling(x);
        }
        
        public static Vec2i Ceil(Vec2 v) {
            return new Vec2i(Ceil(v.X), Ceil(v.Y));
        }
        
        public static double Degrees(double a) {
            return 180 / Math.PI * a;
        }
        
        public static double Radians(double a) {
            return Math.PI / 180 * a;
        }
        
        public static int Mod(int a, int b) {
            int res = a % b;
            if (res < 0)
                res += b;
            return res;
        }
        
        public static double Mod(double a, double b) {
            double res = Math.IEEERemainder(a, b);
            if (res < 0)
                res += b;
            return res;
        }

        public static int Clamp(int x, int a, int b) {
            if (x < a)
                return a;
            if (x > b)
                return b;
            return x;
        }
        public static long Clamp(long x, long a, long b) {
            if (x < a)
                return a;
            if (x > b)
                return b;
            return x;
        }
        public static double Clamp(double x, double a, double b) {
            if (x < a)
                return a;
            if (x > b)
                return b;
            return x;
        }
        public static double Clamp(double x, double a) {
            return Clamp(x, -a, a);
        }
        public static int Clamp(int x, int a) {
            return Clamp(x, -a, a);
        }
        public static long Clamp(long x, long a) {
            return Clamp(x, -a, a);
        }

        public static double AngleDifference(double a1, double a2) {
            double diff = Math.IEEERemainder(a1 - a2, 2 * Math.PI);
            if (diff > Math.PI)
                diff -= 2 * Math.PI;
            return diff;
        }

        public static int DivDown(int a, int b) {
            if (b < 0) { a = -a; b = -b; }
            if (a < 0)
                return -DivUp(-a, b);
            return a / b;
        }

        public static int DivUp(int a, int b) {
            if (b < 0) { a = -a; b = -b; }
            if (a < 0)
                return -DivDown(-a, b);
            return (a + b - 1) / b;
        }

    }

}