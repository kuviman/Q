using System;
using QE;
using QE.Engine;
using QE.EntitySystem;

namespace Q {

    class Client : State {
        public Peer Peer { get; private set; }
        public PlayerData PlayerData { get; set; }

        public Client(string nick, string host) {
            Peer = new Peer(nick, host);
            Peer.AddHandler((string sender, PlayerData playerData) => {
                PlayerData = playerData;
                Peer.ESystem.Add(playerData.MainUnit);
            });

            Systems.Render.Face.SetupHandlers(this);

            cam.Distance = 8;
            cam.UpAngle = -Math.PI / 2.3;
        }

        Camera cam = new Camera(Math.PI / 2);
        Terrain.Renderer terrainRenderer;
        string Room { get { return PlayerData == null ? null : PlayerData.MainUnit.Get<Components.Position>().Room; } }

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
            Peer.Update(dt);
            Peer.CheckMessages();

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
            if (PlayerData != null) {
                PlayerData.MainUnit.Get<Components.Movement>().Vel = new Vec3(v * 10, 0);
                var pos = PlayerData.MainUnit.Get<Components.Position>();
                cam.Position = pos.Pos;
                Vec3 dir = new Vec3(2 * mousePos.X / winSize.Y - (double)winSize.X / winSize.Y, 2 * mousePos.Y / winSize.Y - 1, -1);
                dir.X *= Math.Tan(cam.FOV / 2);
                dir.Y *= Math.Tan(cam.FOV / 2);
                var eyePos = (cam.Matrix.Inverse * Vec4.OrtW).XYZ;
                dir = (cam.Matrix.Inverse * new Vec4(dir, 0)).XYZ;
                // eyePos + dir * t = 0
                double t = -eyePos.Z / dir.Z;
                pos.Rot = (eyePos + dir * t - pos.Pos).XY.Arg;
            }
        }
        public override void MouseMove(Vec2 position) {
            base.MouseMove(position);
            if (MouseButton.Middle.Pressed()) {
                cam.Rotation += (mousePos.X - position.X) / 100;
                cam.UpAngle = GMath.Clamp(cam.UpAngle + (mousePos.Y - position.Y) / 100, -Math.PI / 2, -cam.FOV / 2 - 1e-2);
            }
            mousePos = position;
        }
        Vec2 mousePos;
        Vec2i winSize = new Vec2i(1, 1);

        public event Action<string> OnRenderRoom;
        public override void Render() {
            base.Render();
            winSize = RenderState.Size;
            RenderState.Push();
            Draw.Clear(Color.Sky);
            cam.Apply();
            int x = (int)cam.Position.X;
            int y = (int)cam.Position.Y;
            int dy = GMath.Ceil(cam.Distance * Math.Tan(cam.FOV / 2) * 1.5);
            int dx = GMath.Ceil(dy * RenderState.Aspect);

            if (Room != null) {
                if (Peer.World[Room] == null)
                    Peer.World.AddRoom(Room);
                if (terrainRenderer == null || terrainRenderer.Terrain != Peer.World[Room].Terrain)
                    terrainRenderer = new Terrain.Renderer(Peer.World[Room].Terrain);
                const double D = 30;
                Peer.World[Room].RequestLoaded(cam.Position.X - D, cam.Position.Y - D, cam.Position.X + D, cam.Position.Y + D);
                terrainRenderer.Render(x - dx, y - dy, x + dx + 1, y + dy + 1);
                OnRenderRoom?.Invoke(Room);
            }
            RenderState.Pop();
        }
    }

}