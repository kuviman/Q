using System;
using System.Collections.Generic;
using QE;
using QE.EntitySystem;

namespace Q.Systems {

    static class MovementSytem {

        class Prediction {
            const double LAG = 0.1;

            Components.Position pos;
            Components.Movement mov;

            Vec3 a, b, c, d;

            public Prediction(Components.Position pos, Components.Movement mov) {
                this.pos = pos;
                this.mov = mov;
                a = b = c = Vec3.Zero;
                d = pos.Pos;
            }
            public void Update(Components.Position npos, Components.Movement nmov) {
                k = 0;
                d = pos.Pos;
                c = mov.Vel * LAG;
                // a = pos - b - c - d
                // 3a + 2b + c = vel * LAG
                // 3pos - 3b - 3c - 3d + 2b + c = vel * LAG
                b = 3 * npos.Pos - 2 * c - 3 * d - nmov.Vel * LAG;
                a = npos.Pos - b - c - d;
            }
            double k = 0;
            public void Update(double dt) {
                k += dt / LAG;
                if (k > 1) {
                    mov.Vel = (3 * a + 2 * b + c) / LAG;
                    pos.Pos += mov.Vel * dt;
                } else {
                    mov.Vel = (3 * a * GMath.Sqr(k) + 2 * b * k + c) / LAG;
                    pos.Pos = a * Math.Pow(k, 3) + b * GMath.Sqr(k) + c * k + d;
                }
            }
        }

        static Dictionary<Peer, Dictionary<Entity, Prediction>> predictionMap = new Dictionary<Peer, Dictionary<Entity, Prediction>>();

        public static void SetupHandlers(Peer peer) {
            EGroup movables = new EGroup(peer.ESystem, e => e.Id.StartsWith(peer.Nick + '#') && e.Get<Components.Movement>() != null);
            peer.OnUpdate += dt => {
                foreach (var e in movables.Entities) {
                    var pos = e.Get<Components.Position>();
                    var mov = e.Get<Components.Movement>();
                    pos.Pos += mov.Vel * dt;
                }
            };

            predictionMap[peer] = new Dictionary<Entity, Prediction>();
            peer.OnUpdateEntity += (e, update, updated) => {
                if (update.Get<Components.Movement>() != null) {
                    var predMap = predictionMap[peer];
                    if (!predMap.ContainsKey(e)) {
                        predMap[e] = new Prediction(e.Get<Components.Position>(), e.Get<Components.Movement>());
                    }
                    predMap[e].Update(update.Get<Components.Position>(), update.Get<Components.Movement>());
                    updated.Add(Entity.ComponentName<Components.Position>());
                    updated.Add(Entity.ComponentName<Components.Movement>());
                }
            };
            peer.OnUpdate += dt => {
                foreach (var pred in predictionMap[peer].Values) {
                    pred.Update(dt);
                }
            };
        }
    }

}