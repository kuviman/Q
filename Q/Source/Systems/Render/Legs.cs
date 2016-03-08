using System;
using System.Collections.Generic;
using QE;
using QE.Engine;
using QE.EntitySystem;

namespace Q.Systems.Render {

    static class Legs {
        class State {
            public State(Vec2 pos) {
                this.pos = pos;
            }
            public double dist;
            public Vec2 pos;
            public double h;
        }
        static Dictionary<Peer, Dictionary<Entity, State>> q = new Dictionary<Peer, Dictionary<Entity, State>>();

        public static void SetupHandlers(Client client) {
            q[client.Peer] = new Dictionary<Entity, State>();
            EGroup group = new EGroup(client.Peer.ESystem, e => e.Get<Components.Legs>() != null);
            client.Peer.OnUpdate += dt => {
                foreach (var e in group.Entities) {
                    var pos = e.Get<Components.Position>();
                    if (!q[client.Peer].ContainsKey(e))
                        q[client.Peer][e] = new State(pos.Pos.XY);
                    var s = q[client.Peer][e];
                    double distD = (pos.Pos.XY - s.pos).Length;
                    s.dist += distD;
                    s.pos = pos.Pos.XY;
                    s.h = GMath.Clamp(s.h + 5 * distD - 10 * dt, 0, 1);
                }
            };
            client.OnRenderRoom += (string room) => {
                foreach (var e in group.Entities) {
                    var pos = e.Get<Components.Position>();
                    if (pos.Room != room)
                        continue;
                    if (!q[client.Peer].ContainsKey(e))
                        q[client.Peer][e] = new State(pos.Pos.XY);
                    var s = q[client.Peer][e];
                    RenderState.Push();
                    RenderState.Translate(pos.Pos);
                    RenderState.FaceCam();
                    RenderState.Scale(e.Get<Components.Legs>().Height);
                    RenderState.Color = Color.Black;
                    var sn = Math.Sin(s.dist * 3) * s.h;
                    for (int x = -1; x <= 1; x += 2) {
                        RenderState.Push();
                        RenderState.Translate(x * 2, Math.Max(0, sn * x));
                        //RenderState.Scale(0.1);
                        RenderState.Origin(0, 0.5);
                        Draw.Quad();
                        RenderState.Pop();
                    }
                    RenderState.Pop();
                }
            };
        }

        static Model RotatedModel = new Model(new Shader(Resource.String("Shaders/Rotated.glsl"), "texture", "rotation"));
    }

}