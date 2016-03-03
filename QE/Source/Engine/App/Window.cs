using OpenTK;

namespace QE.Engine {

    partial class App {

        internal static GameWindow window;

        static void InitWindow() {
            var flags = OpenTK.Graphics.GraphicsContextFlags.Default;
#if DEBUG
            Log.Info("Using DEBUG mode for OpenGL");
            flags |= OpenTK.Graphics.GraphicsContextFlags.Debug;
#endif
            Log.Info("Creating window");
            window = new GameWindow(
                640, 480, OpenTK.Graphics.GraphicsMode.Default,
                "QE app", GameWindowFlags.Default, DisplayDevice.Default,
                0, 0, flags, null);
        }

        public static bool VSync {
            get { return window.VSync == VSyncMode.On; }
            set { window.VSync = value ? VSyncMode.On : VSyncMode.Off; }
        }

        public static string Title {
            get { return window.Title; }
            set { window.Title = value; }
        }

        public static int Width { get { return window.Width; } }
        public static int Height { get { return window.Height; } }
        public static Vec2i Size { get { return new Vec2i(window.Width, window.Height); } }
        public static double Aspect { get { return (double)window.Width / window.Height; } }

        public static bool Fullscreen {
            get { return window.WindowState == WindowState.Fullscreen; }
            set { window.WindowState = value ? WindowState.Fullscreen : WindowState.Normal; }
        }

    }

}