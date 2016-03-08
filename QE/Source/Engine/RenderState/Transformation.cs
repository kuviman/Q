namespace QE.Engine {

    partial class RenderState {

        public static Mat4 ModelMatrix {
            get { return Get<Shader.UniformMat4>("modelMatrix").matrix; }
            set { Set("modelMatrix", value); }
        }
        
        public static void MultMatrix(Mat4 matrix) {
            ModelMatrix *= matrix;
        }
        
        public static void Scale(double kx, double ky, double kz) {
            MultMatrix(Mat4.CreateScale(kx, ky, kz));
        }
        public static void Scale(double kx, double ky) {
            Scale(kx, ky, 1);
        }
        public static void Scale(Vec2 k) {
            Scale(k.X, k.Y, 1);
        }
        public static void Scale(Vec3 k) {
            Scale(k.X, k.Y, k.Z);
        }
        public static void Scale(double k) {
            Scale(k, k, k);
        }

        public static void Translate(double dx, double dy, double dz) {
            MultMatrix(Mat4.CreateTranslation(dx, dy, dz));
        }
        public static void Translate(double dx, double dy) {
            Translate(dx, dy, 0);
        }
        public static void Translate(Vec2 dv) {
            Translate(dv.X, dv.Y);
        }
        public static void Translate(Vec3 dv) {
            Translate(dv.X, dv.Y, dv.Z);
        }

        public static void Origin(double x, double y, double z) {
            Translate(-x, -y, -z);
        }
        public static void Origin(double x, double y) {
            Translate(-x, -y);
        }
        public static void Origin(Vec2 origin) {
            Origin(origin.X, origin.Y);
        }
        
        public static void Rotate(double ax, double ay, double az, double angle) {
            MultMatrix(Mat4.CreateRotation(
                new Vec3(ax, ay, az), angle));
        }
        
        public static void RotateX(double angle) {
            Rotate(1, 0, 0, angle);
        }
        public static void RotateY(double angle) {
            Rotate(0, 1, 0, angle);
        }
        public static void RotateZ(double angle) {
            Rotate(0, 0, 1, angle);
        }
        public static void Rotate(double angle) {
            RotateZ(angle);
        }
        
        public static void SetOrts(Vec2 e1, Vec2 e2, Vec2 center) {
            MultMatrix(Mat4.CreateTranslation(center.X, center.Y, 0) * Mat4.CreateFromOrts(new Vec3(e1, 0), new Vec3(e2, 0), Vec3.OrtZ));
        }

        public static void FaceCam() {
            // cam * model * w + (v, 0) = cam * cam-1 * cam * model * 0 + cam * cam-1 * v =
            // = cam * (model * 0 + cam-1 * (v, 0))
            // model * 0 + cam-1 * (v - w) = model * w - cam-1 * w + cam-1 * v
            //ModelMatrix = new Mat4(0, 0, 0, v.X, 0, 0, 0, v.Y, 0, 0, 0, v.Z, 0, 0, 0, 1) + CameraMatrix.Inverse;
            var m = CameraMatrix.Inverse;
            Vec4 v = (ModelMatrix - m) * new Vec4(0, 0, 0, 1);
            m[0, 3] += v.X;
            m[1, 3] += v.Y;
            m[2, 3] += v.Z;
            ModelMatrix = m;
        }

        public static void Billboard() {
            var matrix = ModelMatrix;
            var sx = new Vec3(matrix[0, 0], matrix[0, 1], matrix[0, 2]).Length;
            matrix[0, 0] = sx;
            matrix[0, 1] = 0;
            matrix[0, 2] = 0;
            matrix[1, 1] = matrix[2, 1];
            matrix[1, 0] = matrix[2, 0];
            matrix[1, 2] = matrix[2, 2];
            matrix[2, 2] = 0;
            matrix[2, 0] = 0;
            matrix[2, 1] = 0;
            ModelMatrix = matrix;
        }

    }

}