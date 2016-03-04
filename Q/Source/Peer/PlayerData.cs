using System;
using QE;
using QE.EntitySystem;

namespace Q {

    [Serializable]
    class PlayerData {
        public Entity MainUnit { get; set; }
        public PlayerData(Peer peer) {
            MainUnit = new Entity(peer.GenId());
            MainUnit.Set(new Position("default", Vec3.Zero, 0));
        }
    }

}