using System;
using System.Collections.Generic;
using QE;

namespace Q {

    partial class Terrain {

        public static void SetupHandlers(Peer peer) {
            if (!peer.Server) {
                peer.World.OnNewRoom += (room) => {
                    peer.World[room].OnLoad += (int fromX, int fromY, int toX, int toY) => {
                        peer.SendToServer(new QueryMessage(room, fromX, fromY, toX, toY), QE.Net.DeliveryMethod.Reliable);
                    };
                };
            }

            if (peer.Server) {
                peer.AddHandler((string sender, QueryMessage query) => {
                    peer.World[query.room].RequestLoaded(query.fromX, query.fromY, query.toX, query.toY);
                    Terrain terrain = peer.World[query.room].Terrain;
                    var upd = new UpdateMessage(query.room);
                    for (int i = query.fromX; i < query.toX; i++)
                        for (int j = query.fromY; j < query.toY; j++)
                            upd[i, j] = terrain[i, j];
                    peer.Send(sender, upd, QE.Net.DeliveryMethod.Reliable);
                });
            }

            peer.AddHandler((string sender, UpdateMessage upd) => {
                Terrain terrain = peer.World[upd.room].Terrain;
                foreach (var update in upd.updates)
                    terrain[update.Key.X, update.Key.Y] = update.Value;
            });
        }

        [Serializable]
        public class QueryMessage {
            public string room;
            public int fromX, fromY, toX, toY;
            public QueryMessage(string room, int fromX, int fromY, int toX, int toY) {
                this.room = room;
                this.fromX = fromX;
                this.fromY = fromY;
                this.toX = toX;
                this.toY = toY;
            }
        }

        [Serializable]
        public class UpdateMessage {
            public string room;
            public Dictionary<Vec2i, Vertex> updates = new Dictionary<Vec2i, Vertex>();

            public UpdateMessage(string room) {
                this.room = room;
            }

            public Vertex this[int i, int j] {
                set { updates[new Vec2i(i, j)] = value; }
            }
        }

    }

}
