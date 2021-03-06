﻿using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace QE.Engine {

    partial class Shader {

        Dictionary<string, int> uniformLocations = new Dictionary<string, int>();

        int UniformLocation(string name) {
            if (!uniformLocations.ContainsKey(name))
                uniformLocations[name] = GL.GetUniformLocation(program, name);
            return uniformLocations[name];
        }

        internal interface IUniform {
            void Apply(int location, ref int textures);
        }

        internal class UniformFloat : IUniform {
            public float value;
            public UniformFloat(double value) {
                this.value = (float)value;
            }
            public void Apply(int location, ref int textures) {
                GL.Uniform1(location, value);
            }
        }

        internal class UniformInt : IUniform {
            public int value;
            public UniformInt(int value) {
                this.value = value;
            }
            public void Apply(int location, ref int textures) {
                GL.Uniform1(location, value);
            }
        }

        internal class UniformVec2 : IUniform {
            public float x, y;
            public UniformVec2(double x, double y) {
                this.x = (float)x;
                this.y = (float)y;
            }
            public void Apply(int location, ref int textures) {
                GL.Uniform2(location, x, y);
            }
        }

        internal class UniformVec3 : IUniform {
            public float x, y, z;
            public UniformVec3(double x, double y, double z) {
                this.x = (float)x;
                this.y = (float)y;
                this.z = (float)z;
            }
            public void Apply(int location, ref int textures) {
                GL.Uniform3(location, x, y, z);
            }
        }

        internal class UniformVec4 : IUniform {
            public float x, y, z, w;
            public UniformVec4(double x, double y, double z, double w) {
                this.x = (float)x;
                this.y = (float)y;
                this.z = (float)z;
                this.w = (float)w;
            }
            public void Apply(int location, ref int textures) {
                GL.Uniform4(location, x, y, z, w);
            }
        }

        internal class UniformMat3 : IUniform {
            public Mat3 matrix;
            public UniformMat3(Mat3 matrix) {
                this.matrix = matrix;
            }
            public void Apply(int location, ref int textures) {
                var mat = new OpenTK.Matrix3();
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                        mat[i, j] = (float)matrix[i, j];
                GL.UniformMatrix3(location, false, ref mat);
            }
        }

        internal class UniformMat4 : IUniform {
            public Mat4 matrix;
            public UniformMat4(Mat4 matrix) {
                this.matrix = matrix;
            }
            public void Apply(int location, ref int textures) {
                unsafe {
                    fixed (float* ptr = matrix.a) GL.UniformMatrix4(location, 1, true, ptr);
                }
            }
        }

        internal class UniformTexture : IUniform {
            public Texture texture;
            public UniformTexture(Texture texture) {
                this.texture = texture;
            }
            public void Apply(int location, ref int textures) {
                GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + textures));
                GL.BindTexture(TextureTarget.Texture2D, texture.tex);
                GL.Uniform1(location, textures);
                textures++;
            }
        }

        int uniformTextures;

        HashSet<string> needUniforms;
        public IEnumerable<string> NeedUniforms {
            get { return needUniforms; }
            private set {
                needUniforms = new HashSet<string>(value);
                needUniforms.Add("modelMatrix");
                needUniforms.Add("cameraMatrix");
                needUniforms.Add("projectionMatrix");
            }
        }
        internal void Use() {
            uniformTextures = 0;
            GL.UseProgram(program);
            foreach (string name in NeedUniforms) {
                RenderState.Get<IUniform>(name)?.Apply(UniformLocation(name), ref uniformTextures);
            }
        }

    }

}