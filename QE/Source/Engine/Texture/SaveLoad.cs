using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Reflection;

namespace QE.Engine {

    partial class Texture {
        
        public Texture(System.IO.Stream stream) : this() {
            SetBitmap(new Bitmap(stream));
        }
        
        public Texture(string path) : this() {
            SetBitmap(new Bitmap(path));
        }

        internal void SetBitmap(Bitmap bitmap) {
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.BindTexture(TextureTarget.Texture2D, tex);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                bitmap.Width, bitmap.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);

            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
        }
        
        public void Save(string path) {
            ToBitmap().Save(path);
        }

        internal Bitmap ToBitmap() {
            var bitmap = new Bitmap(Width, Height);
            GL.BindTexture(TextureTarget.Texture2D, tex);
            var data = bitmap.LockBits(new Rectangle(0, 0, Width, Height),
                System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);

            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bitmap;
        }
        
        public Texture Copy() {
            var tex = new Texture();
            tex.Smooth = this.Smooth;
            tex.Wrap = this.Wrap;
            tex.SetBitmap(this.ToBitmap());
            return tex;
        }

    }

}