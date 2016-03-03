using System;
using System.Collections.Generic;

namespace QE.EntitySystem {

    [Serializable]
    public class EGroup {

        Predicate<Entity> filter;

        public EGroup(ESystem esystem, Predicate<Entity> filter) {
            this.filter = filter;
        }

        HashSet<Entity> entities = new HashSet<Entity>();
        public IEnumerable<Entity> Entities {
            get { return entities; }
        }

        void Check(Entity entity) {
            if (filter(entity))
                entities.Add(entity);
            else
                entities.Remove(entity);
        }

    }

}