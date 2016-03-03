using System;
using System.Collections.Generic;
using QE;
using QE.Engine;

namespace Q {

    [Serializable]
    struct ResourcedTexture : IComparable<ResourcedTexture> {
        static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        public string Name { get; private set; }
        public ResourcedTexture(string name) {
            Name = name;
        }

        public Texture Texture {
            get {
                if (!textures.ContainsKey(Name)) {
                    var tex = new Texture(Resource.Stream("Textures/" + Name));
                    tex.Smooth = false;
                    tex.Wrap = Texture.WrapMode.Repeat;
                    textures[Name] = tex;
                }
                return textures[Name];
            }
        }

        public static implicit operator Texture(ResourcedTexture tex) {
            return tex.Texture;
        }

        public int CompareTo(ResourcedTexture other) {
            return Name.CompareTo(other.Name);
        }
    }

}