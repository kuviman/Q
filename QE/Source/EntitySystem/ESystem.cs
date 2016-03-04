using System;
using System.Collections.Generic;

namespace QE.EntitySystem {

    [Serializable]
    public class ESystem {

        public ESystem() { }

        Dictionary<string, Entity> entities = new Dictionary<string, Entity>();
        public IEnumerable<Entity> Entities {
            get {
                return entities.Values;
            }
        }

        public void Add(Entity entity) {
            entities[entity.Id] = entity;
        }

        public void Remove(Entity entity) {
            entities.Remove(entity.Id);
        }

        public Entity this[string id] {
            get {
                if (entities.ContainsKey(id))
                    return entities[id];
                return null;
            }
        }

    }

}