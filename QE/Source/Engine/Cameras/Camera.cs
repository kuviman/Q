using System;

namespace QE.Engine {

    public class Camera {

        public Vec3 Position { get; set; }
        public double Distance { get; set; }
        public double Rotation { get; set; }
        public double UpAngle { get; set; }
        public double FOV { get; set; }

        public Camera(double fov) {
            FOV = fov;
        }

        public void Apply() {
            RenderState.ProjectionMatrix = Mat4.CreatePerspective(FOV, RenderState.Aspect, 1e-2, 1e3);
            RenderState.CameraMatrix = Matrix;
        }

        public Mat4 Matrix {
            get {
                return Mat4.CreateTranslation(0, 0, -Distance)
                    * Mat4.CreateRotationX(-UpAngle - Math.PI / 2)
                    * Mat4.CreateRotationZ(-Rotation)
                    * Mat4.CreateTranslation(-Position.X, -Position.Y, -Position.Z);
            }
        }

    }

}
