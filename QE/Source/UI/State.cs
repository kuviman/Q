using System;

namespace QE.Engine.UI {
    
    public class State : QE.Engine.State {
        
        public QE.Engine.State Background { get; set; }
        public Color? BackgroundColor { get; set; }
        public Frame Frame { get; private set; }
        public Element Focus { get; set; }

        public State() {
            Frame = new Frame();
            Zoom = 1;
        }
        
        public double Zoom { get; set; }
        
        public override void Render() {
            base.Render();
            if (BackgroundColor.HasValue)
                Draw.Clear(BackgroundColor.Value);
            Background?.Render();
            Frame.Size = new Vec2(RenderState.Width, RenderState.Height) / Zoom;
            Frame.Origin = Vec2.Zero;
            for (int i = 0; i < 10; i++)
                Frame.Update(0);
            RenderState.Push();
            RenderState.View2d(0, 0, Frame.Size.X, Frame.Size.Y);
            Frame.Render();
            RenderState.Pop();
        }
        
        public override void Update(double dt) {
            base.Update(dt);
            Background?.Update(dt);
            Frame.Update(dt);
        }
        
        public override void MouseDown(MouseButton button, Vec2 position) {
            base.MouseDown(button, position);
            if (Background != null)
                Background.MouseDown(button, position);
            position = position / Zoom;
            Frame.MouseDown(button, position);

            Element lastFocus = Focus;
            Frame.Visit(elem => {
                if (!elem.Focusable)
                    return;
                if (elem.Inside(position))
                    Focus = elem;
            });

            if (lastFocus != Focus) {
                if (lastFocus != null) {
                    lastFocus.Focused = false;
                    lastFocus.LoseFocus();
                }
                if (Focus != null) {
                    Focus.Focused = true;
                    Focus.Focus();
                }
            }
        }
        
        public override void MouseUp(MouseButton button, Vec2 position) {
            base.MouseUp(button, position);
            Background?.MouseUp(button, position);
            Frame.MouseUp(button, position / Zoom);
        }
        
        public override void MouseMove(Vec2 position) {
            base.MouseMove(position);
            Background?.MouseMove(position);
            Frame.MouseMove(position / Zoom);
        }
        
        public override void KeyDown(Key key) {
            base.KeyDown(key);
            Background?.KeyDown(key);
            Focus?.KeyDown(key);
        }
        
        public override void KeyRepeat(Key key) {
            base.KeyRepeat(key);
            Background?.KeyRepeat(key);
            Focus?.KeyRepeat(key);
        }
        
        public override void KeyUp(Key key) {
            base.KeyUp(key);
            Background?.KeyUp(key);
            Focus?.KeyUp(key);
        }
        
        public override void CharInput(char c) {
            base.CharInput(c);
            Background?.CharInput(c);
            Focus?.CharInput(c);
        }
        
        public override void MouseWheel(double delta) {
            base.MouseWheel(delta);
            Background?.MouseWheel(delta);
        }

    }

}
