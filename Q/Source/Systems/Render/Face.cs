using System;
using QE;
using QE.Engine;
using QE.EntitySystem;

namespace Q.Systems.Render {

    static class Face {
        public static void SetupHandlers(Client client) {
            EGroup group = new EGroup(client.Peer.ESystem, e => e.Get<Components.Face>() != null);
            client.OnRenderRoom += (string room) => {
                foreach (var e in group.Entities) {
                    var pos = e.Get<Components.Position>();
                    if (pos.Room != room)
                        continue;
                    RenderState.Push();
                    RenderState.Translate(pos.Pos);
                    RenderState.FaceCam();
                    RenderState.Set("texture", e.Get<Components.Face>().Texture);
                    RenderState.Set("rotation", -0.25 + (-pos.Rot + (RenderState.CameraMatrix.Inverse * new Vec4(1, 0, 0, 0)).XY.Arg) / (2 * Math.PI));
                    RenderState.Origin(0.5, 0);
                    RotatedModel.Render();
                    RenderState.Color = Color.Black;
                    Draw.Frame(0, 0, 1, 1, 0.05);
                    RenderState.Pop();
                }
            };
        }

        static Model RotatedModel = new Model(new Shader(Resource.String("Shaders/Rotated.glsl"), "texture", "rotation"));
    }

}