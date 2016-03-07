using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace QE.EntitySystem {

    [Serializable]
    public class Entity {

        public string Id { get; private set; }

        public Entity(string id) {
            Id = id;
            InitEvents();
        }

        [OnDeserialized]
        void RestoreEvents(StreamingContext context) {
            InitEvents();
        }

        void InitEvents() {
            OnComponentChanged += (name, c1, c2) => OnChanged?.Invoke();
            foreach (var comp in components.Values)
                comp.OnChanged += OnChanged;
        }
        
        public event Action OnChanged;

        Dictionary<string, IComponent> components = new Dictionary<string, IComponent>();

        public IEnumerable<KeyValuePair<string, IComponent>> Components {
            get { return components; }
        }

        public event Action<string, IComponent, IComponent> OnComponentChanged;
        public void Set(string name, IComponent component) {
            var current = Get(name);
            if (component != null) {
                component.OnChanged += OnChanged;
                components[name] = component;
            } else {
                components.Remove(name);
            }
            OnComponentChanged?.Invoke(name, current, component);
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

        public static string ComponentName<T>() {
            return typeof(T).FullName;
        }

    }

    public interface IComponent {
        event Action OnChanged;
    }

}