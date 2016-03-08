using System;
using OpenTK;

namespace QE {

    [System.Diagnostics.DebuggerDisplay("{ToString()}")]
    [Serializable]
    public unsafe struct Mat4 {

        internal fixed float a[4 * 4];

        public Mat4(double a00, double a01, double a02, double a03,
                    double a10, double a11, double a12, double a13,
                    double a20, double a21, double a22, double a23,
                    double a30, double a31, double a32, double a33) : this() {
            this[0, 0] = a00;
            this[0, 1] = a01;
            this[0, 2] = a02;
            this[0, 3] = a03;

            this[1, 0] = a10;
            this[1, 1] = a11;
            this[1, 2] = a12;
            this[1, 3] = a13;

            this[2, 0] = a20;
            this[2, 1] = a21;
            this[2, 2] = a22;
            this[2, 3] = a23;

            this[3, 0] = a30;
            this[3, 1] = a31;
            this[3, 2] = a32;
            this[3, 3] = a33;
        }

        public override string ToString() {
            string res = "(";
            for (int i = 0; i < 4; i++) {
                if (i > 0)
                    res += ", ";
                res += "(";
                for (int j = 0; j < 4; j++) {
                    if (j > 0)
                        res += ", ";
                    res += this[i, j];
                }
                res += ")";
            }
            return res + ")";
        }

        public Mat4(Vec4 v1, Vec4 v2, Vec4 v3, Vec4 v4) : this(
            v1.X, v1.Y, v1.Z, v1.W,
            v2.X, v2.Y, v2.Z, v2.W,
            v3.X, v3.Y, v3.Z, v3.W,
            v4.X, v4.Y, v4.Z, v4.W) { }

        public double this[int i, int j] {
            get { CheckIndices(i, j); fixed (float* ptr = a) return ptr[i * 4 + j]; }
            set { CheckIndices(i, j); fixed (float* ptr = a) ptr[i * 4 + j] = (float)value; }
        }

        void CheckIndices(int i, int j) {
            if (!(0 <= i && i < 4 && 0 <= j && j < 4))
                throw new IndexOutOfRangeException();
        }

        public static readonly Mat4 Identity = new Mat4(Vec4.OrtX, Vec4.OrtY, Vec4.OrtZ, Vec4.OrtW);

        public static Mat4 operator +(Mat4 matrix) {
            return matrix;
        }
        public static Mat4 operator -(Mat4 matrix) {
            var result = new Mat4();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    result[i, j] = -matrix[i, j];
            return result;
        }

        public static Mat4 operator +(Mat4 a, Mat4 b) {
            Mat4 res = new Mat4();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    res[i, j] = a[i, j] + b[i, j];
            return res;
        }
        public static Mat4 operator -(Mat4 a, Mat4 b) {
            Mat4 res = new Mat4();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    res[i, j] = a[i, j] - b[i, j];
            return res;
        }

        public static Mat4 operator *(Mat4 matrix, double k) {
            Mat4 res = new Mat4();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    res[i, j] = matrix[i, j] * k;
            return res;
        }
        public static Mat4 operator *(double k, Mat4 matrix) {
            Mat4 res = new Mat4();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    res[i, j] = k * matrix[i, j];
            return res;
        }

        public static Mat4 operator /(Mat4 matrix, double k) {
            return matrix * (1 / k);
        }

        public static Mat4 operator *(Mat4 a, Mat4 b) {
            Mat4 res = new Mat4();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    for (int t = 0; t < 4; t++)
                        res[i, j] += a[i, t] * b[t, j];
            return res;
        }

        public static Vec4 operator *(Mat4 a, Vec4 v) {
            Vec4 res = new Vec4();
            for (int i = 0; i < 4; i++) {
                for (int t = 0; t < 4; t++) {
                    res[i] += a[i, t] * v[t];
                }
            }
            return res;
        }

        public static Mat4 CreateTranslation(double dx, double dy, double dz) {
            Mat4 res = Identity;
            res[0, 3] = dx;
            res[1, 3] = dy;
            res[2, 3] = dz;
            return res;
        }
        public static Mat4 CreateTranslation(Vec3 dv) {
            return CreateTranslation(dv.X, dv.Y, dv.Z);
        }

        public static Mat4 CreateScale(double sx, double sy, double sz) {
            Mat4 res = new Mat4();
            res[0, 0] = sx;
            res[1, 1] = sy;
            res[2, 2] = sz;
            res[3, 3] = 1;
            return res;
        }

        public static Mat4 CreateScale(double s) {
            return CreateScale(s, s, s);
        }

        public static Mat4 CreateFromOrts(Vec3 e1, Vec3 e2, Vec3 e3) {
            Mat4 res = new Mat4();
            res[0, 0] = e1.X;
            res[1, 0] = e1.Y;
            res[2, 0] = e1.Z;
            res[0, 1] = e2.X;
            res[1, 1] = e2.Y;
            res[2, 1] = e2.Z;
            res[0, 2] = e3.X;
            res[1, 2] = e3.Y;
            res[2, 2] = e3.Z;
            res[3, 3] = 1;
            return res;
        }

        public static Mat4 CreateRotation(Vec3 u, double angle) {
            double cs = Math.Cos(angle), sn = Math.Sin(angle);
            Mat4 res = new Mat4();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    res[i, j] = u[i] * u[j] * (1 - cs);
            for (int i = 0; i < 3; i++)
                res[i, i] += cs;
            res[0, 1] -= u.Z * sn;
            res[0, 2] += u.Y * sn;
            res[1, 0] += u.Z * sn;
            res[1, 2] -= u.X * sn;
            res[2, 0] -= u.Y * sn;
            res[2, 1] += u.X * sn;
            res[3, 3] = 1;
            return res;
        }
        public static Mat4 CreateRotation(double ux, double uy, double uz, double angle) {
            return CreateRotation(new Vec3(ux, uy, uz), angle);
        }
        public static Mat4 CreateRotationX(double angle) {
            return CreateRotation(1, 0, 0, angle);
        }
        public static Mat4 CreateRotationY(double angle) {
            return CreateRotation(0, 1, 0, angle);
        }
        public static Mat4 CreateRotationZ(double angle) {
            return CreateRotation(0, 0, 1, angle);
        }

        public static Mat4 CreateOrtho(double left, double right, double bottom, double top, double zNear, double zFar) {
            double w = (right - left) / 2;
            double h = (top - bottom) / 2;
            double f = zFar, n = zNear;
            return new Mat4(
                1 / w, 0, 0, 0,
                0, 1 / h, 0, 0,
                0, 0, -2 / (f - n), -(f + n) / (f - n),
                0, 0, 0, 1) * CreateTranslation(-(left + right) / 2, -(bottom + top) / 2, 0);
        }

        public static Mat4 CreatePerspective(double fov, double aspect, double zNear, double zFar) {
            double t = zNear * Math.Tan(fov / 2);
            double r = t * aspect;
            double n = zNear;
            double f = zFar;
            return new Mat4(
                n / r, 0, 0, 0,
                0, n / t, 0, 0,
                0, 0, -(f + n) / (f - n), -2 * f * n / (f - n),
                0, 0, -1, 0);
        }

        public Mat4 Inverse {
            get {
                double[,] a = new double[4, 8];
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++) {
                        a[i, j] = this[i, j];
                        a[i, j + 4] = i == j ? 1 : 0;
                    }

                for (int i = 0; i < 4; i++) {
                    int k = i;
                    for (int j = i + 1; j < 4; j++)
                        if (Math.Abs(a[j, i]) > Math.Abs(a[k, i]))
                            k = j;
                    for (int t = 0; t < 8; t++)
                        GUtil.Swap(ref a[i, t], ref a[k, t]);
                    for (int t = i + 1; t < 8; t++)
                        a[i, t] /= a[i, i];
                    a[i, i] = 1;
                    for (int j = 0; j < 4; j++) {
                        if (i == j) continue;
                        for (int t = i + 1; t < 8; t++)
                            a[j, t] -= a[i, t] * a[j, i];
                        a[j, i] = 0;
                    }
                }

                Mat4 res;
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                        res[i, j] = a[i, j + 4];
                
                return res;
            }
        }

    }

}