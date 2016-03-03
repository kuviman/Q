using System.Collections.Concurrent;
using OpenTK.Graphics.OpenGL;

namespace QE.Engine {

    partial class App {

        static void InitGL() {
            Log.Info("Initializing OpenGL Context");
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
        }

        internal static ConcurrentQueue<int> garbageTextures = new ConcurrentQueue<int>();

        static void FinalizeGLResources() {
            int id;
            while (garbageTextures.TryDequeue(out id)) {
                GL.DeleteTexture(id);
            }
        }

    }

}