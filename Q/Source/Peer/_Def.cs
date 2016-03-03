using System;
using System.Collections.Generic;
using QE;
using Net = QE.Net;

namespace Q {

    class Peer : Net.Peer {
        public World World { get; private set; }

        const string APP_NAME = "QGAME";
        const int APP_PORT = 7827;

        public Peer() : base(APP_NAME, APP_PORT) {
            Init();
            // SERVER

            World.OnNewRoom += (room) => {
                World[room].OnLoad += (int fromX, int fromY, int toX, int toY) => {
                    List<ResourcedTexture> texs = new List<ResourcedTexture>();
                    texs.Add(new ResourcedTexture("Terrain/Grass.png"));
                    texs.Add(new ResourcedTexture("Terrain/DarkGrass.png"));
                    for (int i = fromX; i <= toX; i++)
                        for (int j = fromY; j <= toY; j++) {
                            var v = World[room].Terrain[i, j];
                            v.Texture = GRandom.Choice(texs);
                            World[room].Terrain[i, j] = v;
                        }
                };
            };

            World.AddRoom(Room.DEFAULT);
        }

        public Peer(string nick, string host) : base(APP_NAME, nick, host, APP_PORT) {
            Init();
            // CLIENT

            World.AddRoom(Room.DEFAULT);
        }

        void Init() {
            World = new World();

            Terrain.SetupMessages(this);
        }
    }

}