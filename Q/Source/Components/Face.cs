﻿using System;
using QE.EntitySystem;

namespace Q.Components {

    [Serializable]
    class Face : IComponent {
        public Face(ResourcedTexture texture, double height = 0.2) {
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