using System;
using System.Collections.Generic;

namespace QE.Engine.UI {
    
    public partial class Element {
        
        public Element() {
            InitPositioning();
            InitRendering();
            InitText();
        }

        List<Element> children = new List<Element>();
        
        public IEnumerable<Element> Children { get { return children; } }
        
        public Element Parent { get; private set; }
        
        public void Add(Element child) {
            children.Add(child);
            child.Parent = this;
        }
        
        public void Remove(Element child) {
            children.Remove(child);
            child.Parent = null;
        }
        
        public virtual void Update(double dt) {
            UpdatePosition();
            UpdateText();
            foreach (var child in children)
                child.Update(dt);
        }
        
        public virtual void Render() {
            InternalRender();
            foreach (var child in children)
                child.Render();
            PostRender();
        }
        
        public void Visit(Action<Element> action) {
            if (action == null)
                return;
            action.Invoke(this);
            foreach (var child in Children)
                child.Visit(action);
        }

    }

}
