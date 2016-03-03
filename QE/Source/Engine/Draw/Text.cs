using System;

namespace QE.Engine {

    partial class Draw {

        static void InitText() {
            Font = new Font("Courier New", Engine.Font.Style.Bold);
        }
        
        public static IFont Font { get; set; }
        
        public static void Text(string text) {
            Font.Render(text);
        }
        public static void Text(string text, double ax, double ay = 0) {
            Font.Render(text, ax, ay);
        }

    }

}
