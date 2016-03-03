using System;
using OpenTK.Graphics.OpenGL;

namespace QE.Engine {

    public enum BlendMode {
        Default,
        Add,
        Overwrite,
    }

    partial class RenderState {

        public static Color Color {
            get {
                var uniform = Get<Shader.UniformVec4>("color");
                return new Color(uniform.x, uniform.y, uniform.z, uniform.w);
            }
            set { Set("color", new Shader.UniformVec4(value.R, value.G, value.B, value.A)); }
        }

        class DepthTestSetter : AnywayUniform {
            public bool enable;
            public DepthTestSetter(bool enable) {
                this.enable = enable;
            }
            public override void Apply() {
                GL.DepthFunc(enable ? DepthFunction.Lequal : DepthFunction.Always);
            }
        }
        public static bool DepthTest {
            get { return Get<DepthTestSetter>("!depthTest").enable; }
            set { Set("!depthTest", new DepthTestSetter(value)); }
        }

        class BlendModeSetter : AnywayUniform {
            public BlendMode mode;
            public BlendModeSetter(BlendMode mode) {
                this.mode = mode;
            }
            public override void Apply() {
                switch (mode) {
                case BlendMode.Default:
                    GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                    break;
                case BlendMode.Add:
                    GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);
                    break;
                case BlendMode.Overwrite:
                    GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.Zero);
                    break;
                default:
                    throw new NotImplementedException();
                }
            }
        }
        
        public static BlendMode BlendMode {
            get { return Get<BlendModeSetter>("!blendMode").mode; }
            set { Set("!blendMode", new BlendModeSetter(value)); }
        }

        public static Vec3 ModelToScreen(Vec3 pos) {
            Vec4 v = ProjectionMatrix * CameraMatrix * ModelMatrix * new Vec4(pos.X, pos.Y, pos.Z, 1);
            return v.XYZ / v.W;
        }
        public static Vec3 ModelToScreen(Vec2 pos) {
            return ModelToScreen(new Vec3(pos.X, pos.Y, 0));
        }
        public static Vec3 ModelToScreen(double x, double y, double z = 0) {
            return ModelToScreen(new Vec3(x, y, z));
        }

    }

}