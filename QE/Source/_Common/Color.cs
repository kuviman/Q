using System;

namespace QE {

    [Serializable]
    public struct Color {
        
        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }
        public double A { get; set; }
        
        public Color(double r, double g, double b, double a = 1) : this() {
            R = r;
            G = g;
            B = b;
            A = a;
        }
        
        public override string ToString() {
            return string.Format("({0}; {1}; {2}; {3})", R, G, B, A);
        }

        public static Color FromHSV(double h, double s, double v, double a = 1) {
            h -= Math.Floor(h);
            double r, g, b;
            double f = h * 6 - Math.Floor(h * 6);
            double p = v * (1 - s);
            double q = v * (1 - f * s);
            double t = v * (1 - (1 - f) * s);
            if (h * 6 < 1) {
                r = v; g = t; b = p;
            } else if (h * 6 < 2) {
                r = q; g = v; b = p;
            } else if (h * 6 < 3) {
                r = p; g = v; b = t;
            } else if (h * 6 < 4) {
                r = p; g = q; b = v;
            } else if (h * 6 < 5) {
                r = t; g = p; b = v;
            } else {
                r = v; g = p; b = q;
            }
            return new Color(r, g, b, a);
        }

        Vec3 ToHSV() {
            double Cmax = Math.Max(R, Math.Max(G, B));
            double Cmin = Math.Min(R, Math.Min(G, B));
            double d = Cmax - Cmin;
            double h, s, v;
            if (d == 0.0)
                h = 0.0;
            else if (Cmax == R)
                h = GMath.Mod(((G - B) / d + 6.0) / 6.0, 1.0);
            else if (Cmax == G)
                h = ((B - R) / d + 2.0) / 6.0;
            else
                h = ((R - G) / d + 4.0) / 6.0;
            if (Cmax == 0.0)
                s = 0.0;
            else
                s = d / Cmax;
            v = Cmax;
            return new Vec3(h, s, v);
        }

        public static bool operator ==(Color c1, Color c2) {
            return c1.Equals(c2);
        }
        public static bool operator !=(Color c1, Color c2) {
            return !c1.Equals(c2);
        }

        public double H { get { return ToHSV().X; } }
        public double S { get { return ToHSV().Y; } }
        public double V { get { return ToHSV().Z; } }

        public static readonly Color Black = new Color(0, 0, 0);
        public static readonly Color White = new Color(1, 1, 1);

        public static readonly Color TransparentWhite = new Color(1, 1, 1, 0);
        public static readonly Color TransparentBlack = new Color(0, 0, 0, 0);
        public static readonly Color Transparent = new Color(0, 0, 0, 0);

        public static readonly Color Gray = new Color(0.5, 0.5, 0.5);
        public static readonly Color LightGray = new Color(0.75, 0.75, 0.75);
        public static readonly Color DarkGray = new Color(0.25, 0.25, 0.25);

        public static readonly Color Red = new Color(1, 0, 0);
        public static readonly Color Green = new Color(0, 1, 0);
        public static readonly Color Blue = new Color(0, 0, 1);

        public static readonly Color Yellow = new Color(1, 1, 0);
        public static readonly Color Cyan = new Color(0, 1, 1);
        public static readonly Color Magenta = new Color(1, 0, 1);

        public static readonly Color Orange = new Color(1, 0.5, 0);

        public static readonly Color Sky = new Color(0.8, 0.8, 1);

    }

}