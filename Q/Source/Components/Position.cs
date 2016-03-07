using System;
using QE;
using QE.EntitySystem;

namespace Q.Components {

    [Serializable]
    class Position : IComponent {
        public Position(string room, Vec3 pos, double rot) {
            this.room = room;
            this.pos = pos;
            this.rot = rot;
        }

        Vec3 pos;
        public Vec3 Pos {
            get { return pos; }
            set {
                pos = value;
                OnChanged?.Invoke();
            }
        }

        double rot;
        public double Rot {
            get { return rot; }
            set {
                rot = value;
                OnChanged?.Invoke();
            }
        }

        string room;
        public string Room {
            get { return room; }
            set {
                room = value;
                OnChanged?.Invoke();
            }
        }

        public event Action OnChanged;
    }

}