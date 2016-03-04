using System;
using QE.EntitySystem;

namespace Q {

    static class EntityUpdater {
        public static void SetupHandlers(Peer peer) {
            var ownEntities = new PlayerGroup(peer.ESystem, peer.Nick);
            peer.OnTick += () => {
                foreach (var e in ownEntities.Entities) {
                    peer.SendToAll(e, QE.Net.DeliveryMethod.Default);
                }
            };
            peer.AddHandler((string sender, Entity e) => {
                if (peer.ESystem[e.Id] == null) {
                    peer.ESystem.Add(e);
                } else {
                    foreach (var comp in e.Components)
                        peer.ESystem[e.Id].Set(comp.Key, comp.Value);
                }
            });
        }
    }

}