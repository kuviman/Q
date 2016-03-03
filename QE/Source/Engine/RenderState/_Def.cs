using System;
using System.Collections.Generic;

namespace QE.Engine {

    public static partial class RenderState {

        static RenderState() {
            App.Init();
            ClearState();
        }

        static void ClearState() {
            ProjectionMatrix = Mat4.Identity;
            CameraMatrix = Mat4.Identity;
            ModelMatrix = Mat4.Identity;
            Color = new Color(1, 1, 1, 1);
            DepthTest = false;
            BlendMode = BlendMode.Default;

            anywayUniforms.Add("!depthTest");
            anywayUniforms.Add("!blendMode");
        }

        abstract class AnywayUniform : Shader.IUniform {
            public void Apply(int location, ref int textures) {
                Apply();
            }
            public abstract void Apply();
        }
        static List<string> anywayUniforms = new List<string>();
        internal static void ApplyAnyway() {
            foreach (var name in anywayUniforms)
                Get<AnywayUniform>(name).Apply();
        }

        internal static Dictionary<string, Stack<Tuple<Shader.IUniform, int>>> uniforms =
            new Dictionary<string, Stack<Tuple<Shader.IUniform, int>>>();
        internal static int version = 0;
        
        public static void Push() {
            ++version;
        }
        
        public static void Pop() {
            --version;
            var toRemove = new List<string>();
            foreach (var item in uniforms) {
                while (item.Value.Count > 0 && item.Value.Peek().Item2 > version)
                    item.Value.Pop();
                if (item.Value.Count == 0)
                    toRemove.Add(item.Key);
            }
            foreach (var key in toRemove)
                uniforms.Remove(key);
        }

        internal static T Get<T>(string name) where T : Shader.IUniform {
            return (T)uniforms[name].Peek().Item1;
        }

        static void Set(string name, Shader.IUniform uniform) {
            if (!uniforms.ContainsKey(name))
                uniforms[name] = new Stack<Tuple<Shader.IUniform, int>>();
            uniforms[name].Push(new Tuple<Shader.IUniform, int>(uniform, version));
        }

        public static void Set(string name, double value) {
            Set(name, new Shader.UniformFloat(value));
        }
        public static void Set(string name, int value) {
            Set(name, new Shader.UniformInt(value));
        }
        public static void Set(string name, Vec2 value) {
            Set(name, new Shader.UniformVec2(value.X, value.Y));
        }
        public static void Set(string name, double value1, double value2) {
            Set(name, new Shader.UniformVec2(value1, value2));
        }
        public static void Set(string name, Vec3 value) {
            Set(name, new Shader.UniformVec3(value.X, value.Y, value.Z));
        }
        public static void Set(string name, double value1, double value2, double value3) {
            Set(name, new Shader.UniformVec3(value1, value2, value3));
        }
        public static void Set(string name, double value1, double value2, double value3, double value4) {
            Set(name, new Shader.UniformVec4(value1, value2, value3, value4));
        }
        public static void Set(string name, Mat4 value) {
            Set(name, new Shader.UniformMat4(value));
        }
        public static void Set(string name, Mat3 value) {
            Set(name, new Shader.UniformMat3(value));
        }
        public static void Set(string name, Color value) {
            Set(name, new Shader.UniformVec4(value.R, value.G, value.B, value.A));
        }
        public static void Set(string name, Texture value) {
            Set(name, new Shader.UniformTexture(value));
        }

    }

}