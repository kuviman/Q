using System;

namespace QE.Engine.UI {

    public class StateFrame : Element {

        Engine.State state;

        public StateFrame(Engine.State state) {
            this.state = new Engine.State.Manager(state);
            Focusable = true;
        }

        public StateFrame(Engine.State state, double width, double height) : this(state) {
            Size = new Vec2(width, height);
        }

        public StateFrame(Engine.State state, Vec2 size) : this(state) {
            Size = size;
        }

        public override void MouseDown(MouseButton button, Vec2 position) {
            base.MouseDown(button, position);
            state?.MouseDown(button, MousePos(position));
        }

        public override void MouseMove(Vec2 position) {
            base.MouseMove(position);
            state?.MouseMove(MousePos(position));
        }

        public override void MouseUp(MouseButton button, Vec2 position) {
            base.MouseUp(button, position);
            state?.MouseUp(button, MousePos(position));
        }

        public override void KeyDown(Key key) {
            base.KeyDown(key);
            state?.KeyDown(key);
        }

        public override void KeyRepeat(Key key) {
            base.KeyRepeat(key);
            state?.KeyRepeat(key);
        }

        public override void KeyUp(Key key) {
            base.KeyUp(key);
            state?.KeyUp(key);
        }

        public override void CharInput(char c) {
            base.CharInput(c);
            state?.CharInput(c);
        }

        public override void Update(double dt) {
            base.Update(dt);
            state?.Update(dt);
        }

        static Vec2i getPos(Vec2 v) {
            var result = RenderState.ModelToScreen(v.X, v.Y);
            return new Vec2i((int)(result.X * RenderState.Width + RenderState.Width) / 2,
                (int)(result.Y * RenderState.Height + RenderState.Height) / 2);
        }

        Vec2i renderSize;

        Vec2 MousePos(Vec2 pos) {
            return Vec2.CompMult(Vec2.CompDiv(pos - BottomLeft, Size), renderSize);
        }

        public override void Render() {
            if (state != null) {
                Vec2i p1 = getPos(BottomLeft);
                Vec2i p2 = getPos(TopRight);
                renderSize = p2 - p1;
                RenderState.BeginArea(p1, renderSize);
                state.Render();
                RenderState.EndArea();
            }
            base.Render();
        }

    }

}
