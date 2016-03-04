using System;
using System.Collections.Generic;
using QE;
using Net = QE.Net;
using QE.EntitySystem;

namespace Q {

    class Peer : Net.Peer {
        public World World { get; private set; }

        const string APP_NAME = "QGAME";
        const int APP_PORT = 7827;

        public ESystem ESystem { get; private set; } = new ESystem();

        public Peer() : base(APP_NAME, APP_PORT) {
            Init();
        }

        public Peer(string nick, string host) : base(APP_NAME, nick, host, APP_PORT) {
            Init();
        }

        void Init() {
            World = new World();
            Terrain.SetupHandlers(this);
            EntityUpdater.SetupHandlers(this);
        }

        long NextId = 0;
        public string GenId() {
            return Nick + '#' + (++NextId);
        }

        public double TickTime { get; set; } = 0.05;
        double nextTick = 0;

        public event Action OnTick;
        public virtual void Tick() {
            OnTick?.Invoke();
        }

        public void Update(double dt) {
            nextTick -= dt;
            if (nextTick < 0) {
                nextTick += TickTime;
                if (nextTick < 0)
                    nextTick = 0;
                Tick();
            }
        }
    }

}