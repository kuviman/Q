using System;
using QE.EntitySystem;

namespace Q.Components {

    [Serializable]
    class Face : IComponent {
        public Face(ResourcedTexture texture) {
            this.texture = texture;
        }
        ResourcedTexture texture;
        public ResourcedTexture Texture {
            get { return texture; }
            set {
                texture = value;
                OnChanged?.Invoke();
            }
        }

        public event Action OnChanged;
    }

}