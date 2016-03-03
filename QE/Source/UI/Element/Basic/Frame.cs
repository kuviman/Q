using System;
using System.Collections.Generic;

namespace QE.Engine.UI {
    
    public class Frame : Element {
        
        public Frame() { }

        public Frame(double width, double height) {
            Size = new Vec2(width, height);
        }

        public Frame(Vec2 size) {
            Size = size;
        }

    }

}
