using System;
using OpenTK.Graphics.OpenGL;

namespace QE.Engine {

    partial class Texture {
        
        public void RemoveAlpha() {
            RenderState.BeginTexture(this);
            GL.ColorMask(false, false, false, true);
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ColorMask(true, true, true, true);
            RenderState.EndTexture();
        }

    }

}