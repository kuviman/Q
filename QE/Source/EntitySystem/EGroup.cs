using System;
using System.Collections.Generic;

namespace QE.EntitySystem {

    [Serializable]
    public class EGroup {

        Predicate<Entity> filter;

        public EGroup(ESystem esystem, Predicate<Entity> filter) {
            this.filter = filter;
            foreach (var e in esystem.Entities)
                Check(e);
            esystem.OnAddEntity += Check;
            esystem.OnRemoveEntity += e => entities.Remove(e);
        }

        HashSet<Entity> entities = new HashSet<Entity>();
        public IEnumerable<Entity> Entities {
            get { return entities; }
        }

        public event Action<Entity> OnAddEntity;
        public event Action<Entity> OnRemoveEntity;
        void Check(Entity entity) {
            if (filter(entity)) {
                if (!entities.Contains(entity)) {
                    entities.Add(entity);
                    OnAddEntity?.Invoke(entity);
                }
            } else {
                if (entities.Contains(entity)) {
                    entities.Remove(entity);
                    OnRemoveEntity?.Invoke(entity);
                }
            }
        }

    }

}