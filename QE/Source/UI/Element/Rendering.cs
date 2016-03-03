using System;

namespace QE.Engine.UI {

    partial class Element {

        void InitRendering() {
            BackgroundColor = new Color(0, 0, 0, 0);
            BorderWidth = 2;
            BackgroundColor = new Color(0, 0, 0, 0);
        }
        
        public Color BackgroundColor { get; set; }
        public double BorderWidth { get; set; }
        public Color BorderColor { get; set; }

        void InternalRender() {
            RenderState.Push();
            RenderState.Color = BackgroundColor;
            Draw.Rect(BottomLeft, TopRight);
            RenderState.Color = BorderColor;
            Draw.Frame(BottomLeft, TopRight, BorderWidth);
            RenderState.Pop();
            RenderText();
        }
        
        protected virtual void PostRender() { }

    }

}
