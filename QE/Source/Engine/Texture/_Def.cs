using System;
using OpenTK.Graphics.OpenGL;

namespace QE.Engine {
    
    public partial class Texture : IDisposable {

        static Texture() {
            App.Init();
        }

        internal int tex;

        internal Texture() {
            tex = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, tex);
            Smooth = true;
            Wrap = WrapMode.Clamp;
        }

        ~Texture() {
            Dispose();
        }

        bool disposed = false;
        public void Dispose() {
            if (disposed)
                return;
            App.garbageTextures.Enqueue(tex);
            disposed = true;
        }
        
        public Texture(int width, int height) : this() {
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                width, height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
        }

        public Texture(Vec2i size) : this(size.X, size.Y) { }
        
        public bool Smooth {
            get {
                GL.BindTexture(TextureTarget.Texture2D, tex);
                int filter;
                GL.GetTexParameter(TextureTarget.Texture2D, GetTextureParameter.TextureMinFilter, out filter);
                return (TextureMinFilter)filter == TextureMinFilter.Linear;
            }
            set {
                GL.BindTexture(TextureTarget.Texture2D, tex);
                var minFilter = value ? TextureMinFilter.Linear : TextureMinFilter.Nearest;
                var magFilter = value ? TextureMagFilter.Linear : TextureMagFilter.Nearest;
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);
            }
        }
        
        public WrapMode Wrap {
            get {
                GL.BindTexture(TextureTarget.Texture2D, tex);
                int mode;
                GL.GetTexParameter(TextureTarget.Texture2D, GetTextureParameter.TextureWrapS, out mode);
                return (WrapMode)mode;
            }
            set {
                GL.BindTexture(TextureTarget.Texture2D, tex);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)value);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)value);
            }
        }
        
        public enum WrapMode {
            Clamp = TextureWrapMode.ClampToEdge,
            Repeat = TextureWrapMode.Repeat
        }
        
        public int Width {
            get {
                int value;
                GL.BindTexture(TextureTarget.Texture2D, tex);
                GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out value);
                return value;
            }
        }
        
        public int Height {
            get {
                int value;
                GL.BindTexture(TextureTarget.Texture2D, tex);
                GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out value);
                return value;
            }
        }
        
        public Vec2i Size { get { return new Vec2i(Width, Height); } }
        
        public void Render() {
            RenderState.Push();
            RenderState.Set("texture", this);
            quadModel.Render();
            RenderState.Pop();
        }

        static Shader shader = new Shader(Resource.String("Shaders/Fragment/Texture.glsl"), "color", "texture");
        static Model quadModel = new Model(shader);

    }

}