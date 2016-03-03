using System;

namespace QE.Engine {

    partial class App {

        public static State State { get; private set; }

        public static void Run(State state) {
            State = new State.Manager(state);
            window.Run();
        }

        /// <summary>
        /// Kill the application.
        /// </summary>
        public static void Kill() {
            window.Close();
        }

        /// <summary>
        /// Gets number of frames per second.
        /// </summary>
        /// <value>FPS.</value>
        public static double FPS { get { return timer.FPS; } }

        static Timer timer = new Timer();

        public static event Action OnRender;
        public static event Action<double> OnUpdate;
        public static event Action<Key> OnKeyDown;
        public static event Action<Key> OnKeyUp;
        public static event Action<Key> OnKeyRepeat;
        public static event Action<char> OnCharInput;
        public static event Action<MouseButton, Vec2> OnMouseDown;
        public static event Action<MouseButton, Vec2> OnMouseUp;
        public static event Action<Vec2> OnMouseMove;
        public static event Action<double> OnMouseWheel;

        static void InitEvents() {
            Log.Info("Registering window events");
            window.RenderFrame += (sender, e) => {
                timer.Tick();
                RenderState.SetupViewport();
                OnRender?.Invoke();
                State?.Render();
                window.SwapBuffers();
                FinalizeGLResources();
            };
            window.UpdateFrame += (sender, e) => {
                OnUpdate?.Invoke(e.Time);
                var state = State;
                if (state != null) {
                    state.Update(e.Time);
                    if (state.Closed)
                        State = null;
                }
                if (State == null)
                    Kill();
            };
            window.KeyDown += (sender, e) => {
                var key = (Key)e.Key;
                if (e.IsRepeat) {
                    OnKeyRepeat?.Invoke(key);
                    State?.KeyRepeat(key);
                } else {
                    OnKeyDown?.Invoke(key);
                    State?.KeyDown(key);
                }
                if (e.Key == OpenTK.Input.Key.F4 && e.Modifiers.HasFlag(OpenTK.Input.KeyModifiers.Alt))
                    window.Close();
            };
            window.KeyUp += (sender, e) => {
                OnKeyUp?.Invoke((Key)e.Key);
                State?.KeyUp((Key)e.Key);
            };
            window.KeyPress += (sender, e) => {
                OnCharInput?.Invoke(e.KeyChar);
                State?.CharInput(e.KeyChar);
            };
            window.MouseDown += (sender, e) => {
                OnMouseDown?.Invoke((MouseButton)e.Button, Mouse.Position);
                State?.MouseDown((MouseButton)e.Button, Mouse.Position);
            };
            window.MouseUp += (sender, e) => {
                OnMouseUp?.Invoke((MouseButton)e.Button, Mouse.Position);
                State?.MouseUp((MouseButton)e.Button, Mouse.Position);
            };
            window.MouseMove += (sender, e) => {
                OnMouseMove?.Invoke(Mouse.Position);
                State?.MouseMove(Mouse.Position);
            };
            window.MouseWheel += (sender, e) => {
                OnMouseWheel?.Invoke(e.DeltaPrecise);
                State?.MouseWheel(e.DeltaPrecise);
            };
            window.Resize += (sender, e) => {
                OpenTK.Graphics.OpenGL.GL.Viewport(0, 0, Width, Height);
            };
        }

    }
}