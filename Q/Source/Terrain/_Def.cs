using System;
using System.Collections.Generic;
using QE;
using QE.Engine;

namespace Q {

    [Serializable]
    partial class Terrain {

        const int CHUNK_SIZE = 7;

        [Serializable]
        public struct Vertex {
            public ResourcedTexture Texture { get; set; }
            public double Height { get; set; }
            public double WaterHeight { get; set; }

        }

        Dictionary<Vec2i, Vertex[,]> map = new Dictionary<Vec2i, Vertex[,]>();

        Vertex[,] GetChunk(int x, int y) {
            Vec2i v = new Vec2i(x, y);
            if (!map.ContainsKey(v)) {
                var chunk = new Vertex[CHUNK_SIZE, CHUNK_SIZE];
                for (int i = 0; i < CHUNK_SIZE; i++)
                    for (int j = 0; j < CHUNK_SIZE; j++)
                        chunk[i, j].Texture = new ResourcedTexture("Unknown.png");
                map[v] = chunk;
            }
            return map[v];
        }

        public event Action<int, int, Vertex> OnUpdate;

        public Vertex this[int i, int j] {
            get {
                int cx = GMath.DivDown(i, CHUNK_SIZE);
                int cy = GMath.DivDown(j, CHUNK_SIZE);
                return GetChunk(cx, cy)[i - cx * CHUNK_SIZE, j - cy * CHUNK_SIZE];
            }
            set {
                int cx = GMath.DivDown(i, CHUNK_SIZE);
                int cy = GMath.DivDown(j, CHUNK_SIZE);
                GetChunk(cx, cy)[i - cx * CHUNK_SIZE, j - cy * CHUNK_SIZE] = value;
                OnUpdate?.Invoke(i, j, value);
            }
        }

        public double GetHeight(double x, double y) {
            int i = GMath.Floor(x);
            int j = GMath.Floor(y);
            x -= i;
            y -= j;
            Vec3 v1, v2;
            if (x > y) {
                v1 = GetVec(i + 1, j) - GetVec(i, j);
                v2 = GetVec(i + 1, j + 1) - GetVec(i + 1, j);
            } else {
                v1 = GetVec(i + 1, j + 1) - GetVec(i, j + 1);
                v2 = GetVec(i, j + 1) - GetVec(i, j);
            }
            return GetVec(i, j).Z + v1.Z * x + v2.Z * y;
        }

        Vec3 GetVec(int i, int j) {
            return new Vec3(i, j, this[i, j].Height);
        }

    }

}
