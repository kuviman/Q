using System;
using System.Collections.Generic;

namespace QE.EntitySystem {

    [Serializable]
    public class Entity {

        public string Id { get; private set; }

        public Entity(string id) {
            Id = id;
        }
        public Entity(ESystem esystem) {
            Id = "SERVER";
        }

        Dictionary<string, IComponent> privateComponents = new Dictionary<string, IComponent>();

        [NonSerialized]
        Dictionary<string, IComponent> sharedComponents = new Dictionary<string, IComponent>();
        public void Set(string name, IComponent component) {
            if (component.Shared)
                sharedComponents[name] = component;
            else
                privateComponents[name] = component;
        }
        public IComponent Get(string name) {
            if (privateComponents.ContainsKey(name))
                return privateComponents[name];
            if (sharedComponents.ContainsKey(name))
                return sharedComponents[name];
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
        bool Shared { get; }
    }

    public abstract class Component : IComponent {
        public bool Shared { get; private set; }
        public Component(bool shared) {
            Shared = shared;
        }
    }

}