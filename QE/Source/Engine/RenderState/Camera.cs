namespace QE.Engine {

    partial class RenderState {

        public static Mat4 ProjectionMatrix {
            get { return Get<Shader.UniformMat4>("projectionMatrix").matrix; }
            set { Set("projectionMatrix", value); }
        }

        public static Mat4 CameraMatrix {
            get { return Get<Shader.UniformMat4>("cameraMatrix").matrix; }
            set { Set("cameraMatrix", value); }
        }
        
        public static void View2d(double left, double bottom, double right, double top) {
            ProjectionMatrix = Mat4.CreateOrtho(left, right, bottom, top, -1, 1);
            CameraMatrix = Mat4.Identity;
        }
        public static void View2d(double fov) {
            double h = fov / 2;
            double w = h * Aspect;
            View2d(-w, -h, w, h);
        }
        public static void View2d() {
            View2d(0, 0, Width, Height);
        }

        public static void ViewPerspective(double fov) {
            ProjectionMatrix = Mat4.CreatePerspective(fov, Aspect, 1e-2, 1e3);
        }

        internal static double FontSize {
            get {
                return Height * (ProjectionMatrix * CameraMatrix * ModelMatrix)[1, 1] / 2;
            }
        }

    }

}