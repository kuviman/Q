using System;

namespace QE.Engine {

    public static partial class App {

        static bool initialized = false;

        public static void Init() {
            if (initialized)
                return;
            Log.Info("Initializing QE");
            InitWindow();
            InitEvents();
            InitGL();
            initialized = true;
            VSync = false;
            Log.Info("QE Initialized successfully");
        }

        static App() {
            Init();
        }

    }

}