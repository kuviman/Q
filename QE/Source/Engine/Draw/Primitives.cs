using System;
using System.Collections.Generic;

namespace QE.Engine {

    partial class Draw {

        static Model quadModel = new Model(new[] { new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(0, 1, 0),
            new Vec3(1, 1, 0), new Vec3(1, 0, 0), new Vec3(0, 1, 0)}, Shader.Color);
        
        public static void Quad() {
            quadModel.Render();
        }
        
        public static void Rect(double x1, double y1, double x2, double y2) {
            RenderState.Push();
            RenderState.Translate(x1, y1);
            RenderState.Scale(x2 - x1, y2 - y1);
            Quad();
            RenderState.Pop();
        }
        
        public static void Rect(Vec2 p1, Vec2 p2) {
            Rect(p1.X, p1.Y, p2.X, p2.Y);
        }
        
        public static void Rect(Vec2 p1, Vec2 p2, Color color) {
            RenderState.Push();
            RenderState.Color = color;
            Rect(p1.X, p1.Y, p2.X, p2.Y);
            RenderState.Pop();
        }
        
        public static void Line(Vec2 p1, Vec2 p2, double width) {
            Vec2 v = p2 - p1;
            RenderState.Push();
            RenderState.SetOrts(v, Vec2.Rotate90(v.Unit) * width, p1);
            RenderState.Origin(0, 0.5);
            Quad();
            RenderState.Pop();
        }
        
        public static void Line(double x1, double y1, double x2, double y2, double width) {
            Line(new Vec2(x1, y1), new Vec2(x2, y2), width);
        }
        
        public static void Frame(double x1, double y1, double x2, double y2, double width) {
            Line(x1, y1, x2, y1, width);
            Line(x2, y1, x2, y2, width);
            Line(x2, y2, x1, y2, width);
            Line(x1, y2, x1, y1, width);
        }
        
        public static void Frame(Vec2 p1, Vec2 p2, double width) {
            Frame(p1.X, p1.Y, p2.X, p2.Y, width);
        }
        
        //public static void Circle(double x, double y, double radius) {
        //    RenderState.Push();
        //    RenderState.Translate(x, y);
        //    RenderState.Scale(radius * 2);
        //    RenderState.Origin(0.5, 0.5);
        //    Shader.Std.Circle.RenderQuad();
        //    RenderState.Pop();
        //}
        
        //public static void Circle(Vec2 position, double radius) {
        //    Circle(position.X, position.Y, radius);
        //}

        public static void Box(Texture texture) {
            RenderState.Push();
            RenderState.Translate(0.5, 0.5, 0.5);

            for (int i = 0; i < 4; i++) {
                RenderState.RotateY(Math.PI / 2);
                RenderState.Push();
                RenderState.Origin(0.5, 0.5, 0.5);
                texture.Render();
                RenderState.Pop();
            }

            RenderState.RotateX(Math.PI / 2);
            RenderState.Push();
            RenderState.Origin(0.5, 0.5, 0.5);
            texture.Render();
            RenderState.Pop();

            RenderState.RotateX(Math.PI);
            RenderState.Push();
            RenderState.Origin(0.5, 0.5, 0.5);
            texture.Render();
            RenderState.Pop();

            RenderState.Pop();
        }

        public static void Texture(Texture texture, double x1, double y1, double x2, double y2) {
            RenderState.Push();
            RenderState.Translate(x1, y1);
            RenderState.Scale(x2 - x1, y2 - y1);
            texture.Render();
            RenderState.Pop();
        }

        public static void Texture(Texture texture, Vec2 p1, Vec2 p2) {
            Texture(texture, p1.X, p1.Y, p2.X, p2.Y);
        }

        public static void Polygon(params Vec3[] points) {
            new Model(points, Shader.Color).Render();
        }

        public static void Polygon(params Vec2[] points) {
            Vec3[] vs = new Vec3[points.Length];
            for (int i = 0; i < points.Length; i++)
                vs[i] = new Vec3(points[i].X, points[i].Y, 0);
            Polygon(vs);
        }

    }

}