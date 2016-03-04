using System;
using QE;
using QE.Engine;
using QE.EntitySystem;

namespace Q {

    class Client : State {
        Peer peer;
        public PlayerData PlayerData { get; set; }

        public Client(string nick, string host) {
            peer = new Peer(nick, host);
            peer.AddHandler((string sender, PlayerData playerData) => {
                PlayerData = playerData;
                peer.ESystem.Add(playerData.MainUnit);
            });

            cam.Distance = 8;
            cam.UpAngle = -Math.PI / 2.3;
        }

        Camera cam = new Camera(Math.PI / 2);
        Terrain.Renderer terrainRenderer;
        string Room { get { return PlayerData == null ? null : PlayerData.MainUnit.Get<Position>().Room; } }

        public override void MouseWheel(double delta) {
            base.MouseWheel(delta);
            cam.Distance = GMath.Clamp(cam.Distance - delta, 4, 10);
        }

        public override void KeyDown(Key key) {
            base.KeyDown(key);
            if (key == Key.F)
                App.Fullscreen = !App.Fullscreen;
        }

        public override void Update(double dt) {
            base.Update(dt);
            peer.Update(dt);
            peer.CheckMessages();

            Vec2 v = Vec2.Zero;
            if (Key.W.Pressed())
                v.Y += 1;
            if (Key.S.Pressed())
                v.Y -= 1;
            if (Key.A.Pressed())
                v.X -= 1;
            if (Key.D.Pressed())
                v.X += 1;
            v = Vec2.Rotate(v, cam.Rotation);
            cam.Position += 10 * new Vec3(v, 0) * dt;
            if (PlayerData != null) {
                PlayerData.MainUnit.Get<Position>().Pos = cam.Position;
            }
        }
        
        public override void Render() {
            base.Render();
            RenderState.Push();
            Draw.Clear(Color.Sky);
            cam.Apply();
            int x = (int)cam.Position.X;
            int y = (int)cam.Position.Y;
            int dy = GMath.Ceil(cam.Distance * Math.Tan(cam.FOV / 2) * 1.5);
            int dx = GMath.Ceil(dy * RenderState.Aspect);

            if (Room != null) {
                if (peer.World[Room] == null)
                    peer.World.AddRoom(Room);
                if (terrainRenderer == null || terrainRenderer.Terrain != peer.World[Room].Terrain)
                    terrainRenderer = new Terrain.Renderer(peer.World[Room].Terrain);
                const double D = 30;
                peer.World[Room].RequestLoaded(cam.Position.X - D, cam.Position.Y - D, cam.Position.X + D, cam.Position.Y + D);
                terrainRenderer.Render(x - dx, y - dy, x + dx + 1, y + dy + 1);

                foreach (var e in peer.ESystem.Entities) {
                    var pos = e.Get<Position>();
                    if (pos == null)
                        continue;
                    RenderState.Push();
                    RenderState.Translate(pos.Pos);
                    RenderState.Rotate(pos.Rot);
                    RenderState.Origin(0.5, 0.5);
                    RenderState.Color = Color.Black;
                    Draw.Quad();
                    Draw.Text(e.Id.Substring(0, e.Id.IndexOf('#')), 0.5, -1);
                    RenderState.Pop();
                }
            }
            RenderState.Pop();
        }
    }

}