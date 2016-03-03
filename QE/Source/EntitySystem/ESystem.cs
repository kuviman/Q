using System;
using System.Collections.Generic;

namespace QE.EntitySystem {

    [Serializable]
    public class ESystem {

        public string Name { get; private set; }
        public ESystem(string name) {
            Name = name;
        }
        public ESystem() {
            Name = "SERVER";
        }

        int NextId = 0;
        public string GenId() {
            return Name + '#' + (NextId++);
        }

        Dictionary<string, Entity> privateEntities = new Dictionary<string, Entity>();
        Dictionary<string, Entity> sharedEntities = new Dictionary<string, Entity>(); // Send to other
        public IEnumerable<Entity> Entities {
            get {
                foreach (var entity in privateEntities.Values)
                    yield return entity;
                foreach (var entity in sharedEntities.Values)
                    yield return entity;
            }
        }

        public void Add(Entity entity) {
            if (entity.Id.StartsWith(Name))
                privateEntities[entity.Id] = entity;
            else
                sharedEntities[entity.Id] = entity;
        }

        public void Remove(Entity entity) {
            privateEntities.Remove(entity.Id);
            sharedEntities.Remove(entity.Id);
        }

    }

}