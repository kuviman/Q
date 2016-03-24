using System;
using QE.EntitySystem;

namespace Q.Components {

    [Serializable]
    class Image : IComponent {
        public Image(ResourcedTexture texture, double height) {
            this.texture = texture;
            this.height = height;
        }
        ResourcedTexture texture;
        public ResourcedTexture Texture {
            get { return texture; }
            set {
                texture = value;
                OnChanged?.Invoke();
            }
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