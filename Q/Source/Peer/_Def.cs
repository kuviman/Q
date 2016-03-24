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
            Systems.MovementSytem.SetupHandlers(this);
            SetupEntityUpdateHandlers();
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

        public event Action<double> OnUpdate;
        public void Update(double dt) {
            nextTick -= dt;
            if (nextTick < 0) {
                nextTick += TickTime;
                if (nextTick < 0)
                    nextTick = 0;
                BeginMessagePack();
                Tick();
                EndMessagePack();
            }
            OnUpdate?.Invoke(dt);
        }

        public event Action<Entity, Entity, ISet<string>> OnUpdateEntity;
        public void UpdateEntity(Entity e, Entity update) {
            HashSet<string> updated = new HashSet<string>();
            OnUpdateEntity?.Invoke(e, update, updated);
            foreach (var comp in update.Components) {
                if (updated.Contains(comp.Key))
                    continue;
                e.Set(comp.Key, comp.Value);
            }
        }

        HashSet<Entity> updatedEntities = new HashSet<Entity>();
        void SetupEntityUpdateHandlers() {
            var ownEntities = new PlayerGroup(ESystem, Nick);
            OnTick += () => {
                foreach (var e in updatedEntities)
                    SendToAll(e, Net.DeliveryMethod.Default);
                foreach (var e in ownEntities.Entities)
                    SendToAll(new EntityId(e.Id), Net.DeliveryMethod.Default);
            };
            foreach (var e in ownEntities.Entities)
                e.OnChanged += () => updatedEntities.Add(e);
            ownEntities.OnAddEntity += e => e.OnChanged += () => updatedEntities.Add(e);
            AddHandler((string sender, Entity e) => {
                if (ESystem[e.Id] == null) {
                    ESystem.Add(e);
                } else {
                    UpdateEntity(ESystem[e.Id], e);
                }
            });
            AddHandler((string sender, EntityId eId) => {
                if (ESystem[eId.id] == null) {
                    Send(sender, new EntityRequest(eId.id), Net.DeliveryMethod.Default);
                }
            });
            AddHandler((string sender, EntityRequest req) => {
                 Send(sender, ESystem[req.id], Net.DeliveryMethod.Default);
            });
        }

        [Serializable]
        class EntityId {
            public string id;
            public EntityId(string id) {
                this.id = id;
            }
        }
        [Serializable]
        class EntityRequest {
            public string id;
            public EntityRequest(string id) {
                this.id = id;
            }
        }
    }

}