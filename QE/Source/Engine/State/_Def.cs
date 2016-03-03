using System;

namespace QE.Engine {
    
    public partial class State {
        
        public bool Closed { get; private set; }

        public event Action OnClose;
        public virtual void Close() {
            if (!Closed)
                OnClose?.Invoke();
            Closed = true;
        }

        public event Action OnRender;
        public virtual void Render() {
            OnRender?.Invoke();
        }

        public event Action<double> OnUpdate;
        public virtual void Update(double dt) {
            OnUpdate?.Invoke(dt);
        }

        public event Action<Key> OnKeyDown;
        public virtual void KeyDown(Key key) {
            OnKeyDown?.Invoke(key);
        }

        public event Action<Key> OnKeyRepeat;
        public virtual void KeyRepeat(Key key) {
            OnKeyRepeat?.Invoke(key);
        }

        public event Action<Key> OnKeyUp;
        public virtual void KeyUp(Key key) {
            OnKeyUp?.Invoke(key);
        }

        public event Action<char> OnCharInput;
        public virtual void CharInput(char c) {
            OnCharInput?.Invoke(c);
        }

        public event Action<MouseButton, Vec2> OnMouseDown;
        public virtual void MouseDown(MouseButton button, Vec2 position) {
            OnMouseDown?.Invoke(button, position);
        }

        public event Action<MouseButton, Vec2> OnMouseUp;
        public virtual void MouseUp(MouseButton button, Vec2 position) {
            OnMouseUp?.Invoke(button, position);
        }

        public event Action<Vec2> OnMouseMove;
        public virtual void MouseMove(Vec2 position) {
            OnMouseMove?.Invoke(position);
        }

        public event Action<double> OnMouseWheel;
        public virtual void MouseWheel(double delta) {
            OnMouseWheel?.Invoke(delta);
        }
    }

}