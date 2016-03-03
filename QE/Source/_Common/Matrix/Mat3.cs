using System;
using OpenTK;

namespace QE {
    
    [Serializable]
    public struct Mat3 {

        Matrix3d mat;
        Mat3(Matrix3d mat) {
            this.mat = mat;
        }

        public double this[int i, int j] {
            get { return mat[i, j]; }
            set { mat[i, j] = value; }
        }
        
        public override string ToString() {
            return mat.ToString();
        }

        public static readonly Mat3 Identity = new Mat3(Matrix3d.Identity);

        public static Mat3 operator *(Mat3 a, Mat3 b) {
            return new Mat3(a.mat * b.mat);
        }

    }

}