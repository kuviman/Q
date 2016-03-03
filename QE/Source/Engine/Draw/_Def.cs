using System;
using OpenTK.Graphics.OpenGL;

namespace QE.Engine {
    
    public static partial class Draw {

        static Draw() {
            App.Init();
            InitText();
        }
        
        public static void Clear(double r, double g, double b, double a = 1) {
            GL.ClearColor((float)r, (float)g, (float)b, (float)a);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
        
        public static void Clear(Color color) {
            Clear(color.R, color.G, color.B, color.A);
        }

        public static void ClearDepth() {
            GL.Clear(ClearBufferMask.DepthBufferBit);
        }

    }

}