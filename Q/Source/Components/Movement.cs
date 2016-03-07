using System;
using QE;
using QE.EntitySystem;

namespace Q.Components {

    [Serializable]
    class Movement : IComponent {
        Vec3 vel = Vec3.Zero;
        public Vec3 Vel {
            get { return vel; }
            set {
                vel = value;
                OnChanged?.Invoke();
            }
        }

        public event Action OnChanged;
    }

}