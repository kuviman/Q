using System;
using QE.EntitySystem;

namespace Q.Components {

    [Serializable]
    class Legs : IComponent {
        public Legs(double height = 0.1) {
            this.height = height;
        }

        double height;
        public double Height {
            get { return height; }
            set {
                height = value;
                OnChanged?.Invoke();
            }
        }

        public event Action OnChanged;
    }

}