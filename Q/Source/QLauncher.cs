using System;
using System.Collections.Generic;
using QE;
using QE.Engine;
using UI = QE.Engine.UI;

namespace Q {

    class Game : State {
        Peer peer;
        string room;

        Camera cam = new Camera(Math.PI / 2);
        Terrain.Renderer terrainRenderer;

        public Game(Peer peer) {
            this.peer = peer;
            room = Room.DEFAULT;
            cam.Distance = 8;
            cam.UpAngle = -Math.PI / 2.3;
            terrainRenderer = new Terrain.Renderer(peer.World[room].Terrain);
        }

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
            peer.CheckMessages();

            const double D = 30;
            peer.World[room].RequestLoaded(cam.Position.X - D, cam.Position.Y - D, cam.Position.X + D, cam.Position.Y + D);

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
            terrainRenderer.Render(x - dx, y - dy, x + dx + 1, y + dy + 1);
            RenderState.Pop();
        }
    }

    class Test : UI.State {
        Peer peer;

        Terrain terrain;
        Terrain.Renderer terrainRenderer;
        Camera cam = new Camera(Math.PI / 2);

        UI.Element fps = new UI.Element();

        public Test(string nick, string host) {
            peer = new Peer(nick, host);
            cam.Distance = 8;
            cam.UpAngle = -Math.PI / 2.3;
            cam.Rotation = 0.1;
            terrain = new Terrain();
            terrainRenderer = new Terrain.Renderer(terrain);

            List<ResourcedTexture> ts = new List<ResourcedTexture>();
            List<Color> cs = new List<Color>();

            ts.Add(new ResourcedTexture("Terrain/DarkGrass.png"));
            cs.Add(Color.Black);

            ts.Add(new ResourcedTexture("Terrain/Grass.png"));
            cs.Add(Color.White);

            ts.Add(new ResourcedTexture("Terrain/Sand.png"));
            cs.Add(Color.Yellow);

            ts.Add(new ResourcedTexture("Terrain/WoodenFloor.png"));
            cs.Add(Color.Red);

            var tex = new Texture(20, 20);
            RenderState.BeginTexture(tex);
            Draw.Clear(Color.Black);
            RenderState.View2d();
            RenderState.Color = Color.White;
            for (int i = 0; i < 10; i++) {
                Vec2 v1 = new Vec2(GRandom.NextDouble(0, tex.Width), GRandom.NextDouble(0, tex.Height));
                Vec2 v2 = new Vec2(GRandom.NextDouble(0, tex.Width), GRandom.NextDouble(0, tex.Height));
                Draw.Line(v1, v2, 10);
            }
            RenderState.EndTexture();
            for (int i = 0; i < tex.Width; i++)
                for (int j = 0; j < tex.Height; j++) {
                    int x = i - tex.Width / 2, y = j - tex.Height / 2;
                    var v = terrain[x, y];
                    Color c = tex[i, j];
                    for (int t = 0; t < ts.Count; t++)
                        if (c == cs[t])
                            v.Texture = ts[t];
                    v.Height = GRandom.NextDouble(0, 0.2);
                    terrain[x, y] = v;
                }

            fps.BackgroundColor = new Color(0, 0, 0, 0.5);
            fps.TextColor = Color.White;
            fps.Anchor = fps.Origin = new Vec2(1, 1);
            Frame.Add(fps);
        }

        public override void Update(double dt) {
            base.Update(dt);

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
        }

        public override void MouseWheel(double delta) {
            base.MouseWheel(delta);
            cam.Distance -= delta;
        }

        public override void KeyDown(Key key) {
            base.KeyDown(key);
            if (key == Key.F)
                App.Fullscreen = !App.Fullscreen;
        }

        public override void Render() {
            RenderState.Push();
            Draw.Clear(Color.Sky);
            cam.Apply();
            //RenderState.View2d(15);
            int x = (int)cam.Position.X;
            int y = (int)cam.Position.Y;
            int dy = GMath.Ceil(cam.Distance * Math.Tan(cam.FOV / 2) * 1.5);
            int dx = GMath.Ceil(dy * RenderState.Aspect);
            terrainRenderer.Render(x - dx, y - dy, x + dx + 1, y + dy + 1);
            RenderState.Pop();

            fps.Text = "FPS: " + ((int)App.FPS).ToString();
            App.Title = peer.Connected.ToString() + ": " + string.Join(", ", peer.ConnectedPlayers);

            base.Render();
        }

        Vec2 startDrag;
        public override void MouseMove(Vec2 position) {
            base.MouseMove(position);
            if (MouseButton.Middle.Pressed()) {
                Vec2 dv = (position - startDrag) / 100;
                cam.Rotation -= dv.X;
                cam.UpAngle = GMath.Clamp(cam.UpAngle + dv.Y, -Math.PI / 2, -Math.PI / 3);
            }
            startDrag = position;
        }
    }

    class Server {
        Peer peer = new Peer();

        public void Run() {
            Timer timer = new Timer();
            while (true) {
                peer.CheckMessages();
                timer.Tick();
            }
        }
    }

    class QLauncher {
        static void Main(string[] args) {
            var ops = new CmdOp(args);
            Log.Info(ops.ToString());
            if (ops["server"] != null) {
                new Server().Run();
            } else if (ops["connect"] != null) {
                App.Run(new Game(new Peer(ops["nick"], ops["connect"])));
            } else {
                GUtil.StartThread(() => new Server().Run());
                App.Run(new Game(new Peer("kuviman", "127.0.0.1")));
            }
        }
    }

}