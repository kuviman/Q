using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace QE.Engine {

    public class Model {

        public Shader Shader { get; set; }

        List<Vec3> vs;
        public IEnumerable<Vec3> Vertices { get { return vs; } }

        int VAO, VBO;

        public Model(IEnumerable<Vec3> vertices, Shader shader) {
            Init(vertices, shader);
        }
        public Model(Shader shader) {
            Init(new[] {new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(0, 1, 0),
                        new Vec3(1, 1, 0), new Vec3(0, 1, 0), new Vec3(1, 0, 0)}, 
                        shader);
        }

        void Init(IEnumerable<Vec3> vertices, Shader shader) {
            Shader = shader;
            vs = new List<Vec3>(vertices);
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            float[] data = new float[vs.Count * 3];
            for (int i = 0; i < vs.Count; i++) {
                data[i * 3] = (float)vs[i].X;
                data[i * 3 + 1] = (float)vs[i].Y;
                data[i * 3 + 2] = (float)vs[i].Z;
            }
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
        }

        void InitAttr(int loc) {
            if (loc < 0) return;
            GL.VertexAttribPointer(loc, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
            GL.EnableVertexAttribArray(loc);
        }

        public void Render() {
            Shader.Use();
            RenderState.ApplyAnyway();

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BindVertexArray(VAO);
            int posLoc = GL.GetAttribLocation(Shader.program, "position");
            InitAttr(posLoc);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vs.Count);

            //GL.Begin(BeginMode.Triangles);
            //foreach (var pos in Vertices)
            //    GL.Vertex3(pos.X, pos.Y, pos.Z);
            //GL.End();
        }

        static Model() {
            App.Init();
        }

    }

}