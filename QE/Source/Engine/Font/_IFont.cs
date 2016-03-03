using System;

namespace QE.Engine {
    
    public interface IFont {
        double Measure(string text);
        void Render(string text);
    }
    
    public static class FontExt {

        public static void Render(this IFont font, string text, double ax, double ay = 0) {
            RenderState.Push();
            RenderState.Origin(ax * font.Measure(text), ay);
            font.Render(text);
            RenderState.Pop();
        }

    }

}