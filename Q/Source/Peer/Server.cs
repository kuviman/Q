using System;
using System.Collections.Generic;
using QE;
using QE.EntitySystem;

namespace Q {

    class Server : Peer {
        Dictionary<string, PlayerData> playerData = new Dictionary<string, PlayerData>();

        public Server() : base() {
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
            OnPlayerConnected += (string player) => {
                if (!playerData.ContainsKey(player))
                    playerData[player] = new PlayerData(player, this);
                Send(player, playerData[player], QE.Net.DeliveryMethod.Reliable);
            };

            World.AddRoom(Room.DEFAULT);
        }

        public void Run() {
            Timer timer = new Timer();
            while (true) {
                CheckMessages();
                Update(timer.Tick());
            }
        }

    }

}