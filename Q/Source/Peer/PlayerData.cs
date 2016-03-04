using System;
using QE;
using QE.EntitySystem;

namespace Q {

    [Serializable]
    class PlayerData {
        public Entity MainUnit { get; set; }
        public PlayerData(string player, Peer peer) {
            MainUnit = new Entity(player + "#Hero");
            MainUnit.Set(new Position("default", Vec3.Zero, 0));
        }
    }

}