using System;

namespace QE.Engine.UI {

    partial class Element {
        
        public virtual bool Inside(Vec2 pos) {
            Vec2 a = BottomLeft, b = TopRight;
            return a.X <= pos.X && pos.X <= b.X
                && a.Y <= pos.Y && pos.Y <= b.Y;
        }

        Element mouseFocus = null;
        
        public bool Pressed { get; set; }
        
        public event Action<MouseButton, Vec2> OnMouseDown;
        public virtual void MouseDown(MouseButton button, Vec2 position) {
            foreach (var child in children) {
                if (child.Inside(position)) {
                    child.MouseDown(button, position);
                    mouseFocus = child;
                }
            }
            if (!Pressed)
                Press();
            Pressed = true;
            OnMouseDown?.Invoke(button, position);
        }
        
        public event Action<MouseButton, Vec2> OnMouseUp;
        public virtual void MouseUp(MouseButton button, Vec2 position) {
            foreach (var child in children) {
                if (child.Inside(position) || mouseFocus == child)
                    child.MouseUp(button, position);
            }
            mouseFocus = null;
            if (Pressed) {
                Release();
                if (Inside(position))
                    Click();
            }
            Pressed = false;
            OnMouseUp?.Invoke(button, position);
        }
        
        public event Action OnPress;
        public virtual void Press() {
            OnPress?.Invoke();
        }
        
        public event Action OnRelease;
        public virtual void Release() {
            OnRelease?.Invoke();
        }
        
        public event Action OnClick;
        public virtual void Click() {
            OnClick?.Invoke();
        }
        
        public event Action<Vec2> OnMouseMove;
        public virtual void MouseMove(Vec2 position) {
            foreach (var child in children) {
                child.MouseMove(position);
                if (child.Inside(position)) {
                    if (!child.Hovered)
                        child.Hover();
                    child.Hovered = true;
                } else {
                    if (child.Hovered)
                        child.Unhover();
                    child.Hovered = false;
                }
            }
            OnMouseMove?.Invoke(position);
        }
        
        public bool Hovered { get; set; }
        public event Action OnHover;
        public virtual void Hover() {
            OnHover?.Invoke();
        }
        public event Action OnUnhover;
        public virtual void Unhover() {
            OnUnhover?.Invoke();
        }
        
        public bool Focusable { get; set; }
        public bool Focused { get; set; }
        public event Action OnFocus;
        public virtual void Focus() {
            OnFocus?.Invoke();
        }
        public event Action OnLoseFocus;
        public virtual void LoseFocus() {
            OnLoseFocus?.Invoke();
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

    }

}
