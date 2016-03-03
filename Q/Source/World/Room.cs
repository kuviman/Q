using System;
using System.Collections.Generic;
using QE;

namespace Q {

    [Serializable]
    class Room {
        const int CHUNK_SIZE = 8;
        public const string DEFAULT = "default";

        HashSet<Vec2i> loadedChunks = new HashSet<Vec2i>();

        public event Action<int, int, int, int> OnLoad;

        Terrain terrain = new Terrain();
        public Terrain Terrain { get { return terrain; } }

        public void RequestLoaded(double fromX, double fromY, double toX, double toY) {
            for (int i = GMath.DivDown(GMath.Floor(fromX), CHUNK_SIZE); i < GMath.DivUp(GMath.Ceil(toX), CHUNK_SIZE); i++)
                for (int j = GMath.DivDown(GMath.Floor(fromY), CHUNK_SIZE); j < GMath.DivUp(GMath.Ceil(toY), CHUNK_SIZE); j++) {
                    if (!loadedChunks.Contains(new Vec2i(i, j))) {
                        OnLoad?.Invoke(i * CHUNK_SIZE, j * CHUNK_SIZE, (i + 1) * CHUNK_SIZE, (j + 1) * CHUNK_SIZE);
                        loadedChunks.Add(new Vec2i(i, j));
                    }
                }
        }
    }

}