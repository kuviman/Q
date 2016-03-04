using System;
using System.Collections.Generic;

namespace QE.EntitySystem {

    [Serializable]
    public class Entity {

        public string Id { get; private set; }

        public Entity(string id) {
            Id = id;
        }

        public event Action OnChanged;

        Dictionary<string, IComponent> components = new Dictionary<string, IComponent>();
        public void Set(string name, IComponent component) {
            component.OnChanged += OnChanged;
            components[name] = component;
        }
        public IComponent Get(string name) {
            if (components.ContainsKey(name))
                return components[name];
            return null;
        }

        public void Set<T>(T component) where T : IComponent {
            Set(typeof(T).FullName, component);
        }

        public T Get<T>() where T : IComponent {
            return (T)Get(typeof(T).FullName);
        }

        public bool Has(string name) {
            return Get(name) != null;
        }
        public bool Has<T>() where T : IComponent {
            return Has(typeof(T).FullName);
        }

    }

    public interface IComponent {
        event Action OnChanged;
    }

}