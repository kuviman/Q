using System;
using QE;
using QE.Engine;
using QE.EntitySystem;

namespace Q.Systems.Render {

    static class Image {
        public static void SetupHandlers(Client client) {
            EGroup group = new EGroup(client.Peer.ESystem, e => e.Get<Components.Image>() != null);
            client.OnRenderRoom += (string room) => {
                foreach (var e in group.Entities) {
                    var pos = e.Get<Components.Position>();
                    if (pos.Room != room)
                        continue;
                    var sp = RenderState.ModelToScreen(pos.Pos);
                    const double DELTA = 0;
                    if (sp.X < -1 - DELTA || sp.X > 1 + DELTA || sp.Y < -1 - DELTA || sp.Y > 1 + DELTA)
                        continue;
                    RenderState.Push();
                    RenderState.Translate(pos.Pos);
                    RenderState.FaceCam();
                    var image = e.Get<Components.Image>();
                    var tex = image.Texture.Texture;
                    RenderState.Scale(tex.Width * image.Height / tex.Height, image.Height);
                    RenderState.Origin(0.5, 0);
                    tex.Render();
                    RenderState.Pop();
                }
            };
        }
    }

}