using System;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace QE.Engine {
    
    public partial class Shader {

        static int vertexShader;
        static Shader() {
            App.Init();
            Log.Info("Compiling basic vertex shader");
            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, Resource.String("Shaders/Vertex/Basic.glsl"));
            GL.CompileShader(vertexShader);

            AddLib(Resource.String("Shaders/Lib/HSV.glsl"));
            AddLib(Resource.String("Shaders/Lib/Texture.glsl"));

            Color = new Shader(Resource.String("Shaders/Fragment/Color.glsl"), "color");
        }

        public static readonly Shader Color;

        static List<string> libSources = new List<string>();
        public static void AddLib(string source) {
            libSources.Add(source + "\n");
        }

        internal int program;
        public Shader(IEnumerable<string> sources, IEnumerable<string> needUniforms) {
            NeedUniforms = needUniforms;

            program = GL.CreateProgram();
            foreach (var source in sources) {
                var shader = GL.CreateShader(ShaderType.FragmentShader);
                var completeSource = source;
                var version = "";
                if (completeSource.StartsWith("#version")) {
                    int index = completeSource.IndexOf("\n");
                    version = completeSource.Substring(0, index + 2);
                    completeSource = completeSource.Remove(0, index + 2);
                }
                foreach (var lib in libSources)
                    completeSource = lib + completeSource;
                completeSource = version + completeSource;

                GL.ShaderSource(shader, completeSource);
                GL.CompileShader(shader);
                int compileStatus;
                GL.GetShader(shader, ShaderParameter.CompileStatus, out compileStatus);
                if (compileStatus == 0) {
                    throw new OpenTK.GraphicsException(GL.GetShaderInfoLog(shader));
                }
                GL.AttachShader(program, shader);
            }
            GL.AttachShader(program, vertexShader);
            GL.LinkProgram(program);
            int linkStatus;
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out linkStatus);
            if (linkStatus == 0) {
                throw new OpenTK.GraphicsException(GL.GetProgramInfoLog(program));
            }
        }

        public Shader(string source, params string[] needUniforms) : this(new[] { source }, needUniforms) { }

    }

}