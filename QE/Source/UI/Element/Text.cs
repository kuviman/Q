using System;

namespace QE.Engine.UI {

    partial class Element {

        void InitText() {
            TextSize = 20;
            TextAlign = 0.5;
            TextColor = new Color(1, 1, 1, 1);
        }
        
        public string Text { get; set; }
        public double TextSize { get; set; }
        public double TextAlign { get; set; }
        public Color TextColor { get; set; }
        public IFont Font { get; set; }

        internal IFont RealFont { get { return Font == null ? Draw.Font : Font; } }
        
        public double? FixedWidth { get; set; }

        void UpdateText() {
            if (Text == null)
                return;
            var padding = Padding * TextSize;
            Size = new Vec2(padding, padding) * 2;
            foreach (var line in Text.Split('\n'))
                Size = new Vec2(Math.Max(Size.X, padding * 2 + TextSize * RealFont.Measure(line)), Size.Y + TextSize);
            if (FixedWidth != null)
                Size = new Vec2(FixedWidth.Value, Size.Y);
        }

        void RenderText() {
            if (Text == null)
                return;
            RenderState.Push();
            IFont font = RealFont;
            var padding = Padding * TextSize;
            RenderState.Translate(TopLeft + new Vec2(TextAlign * (Size.X - 2 * padding) + padding, -padding));
            RenderState.Scale(TextSize);
            RenderState.Color = TextColor;
            foreach (var line in Text.Split('\n')) {
                RealFont.Render(line, TextAlign, 1);
                RenderState.Translate(0, -1);
            }

            RenderState.Pop();
        }

    }

}
