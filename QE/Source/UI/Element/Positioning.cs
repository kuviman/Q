using System;

namespace QE.Engine.UI {

    partial class Element {

        void InitPositioning() {
            Margin = 0.1;
            Padding = 0.2;
            Size = new Vec2(1, 1);
            Origin = new Vec2(0.5, 0.5);
        }
        
        public double Margin { get; set; }
        public double Padding { get; set; }
        public Vec2 Size { get; set; }
        public Vec2 Position { get; set; }
        
        public Vec2 BottomLeft {
            get { return Position - Vec2.CompMult(Size, Origin); }
            set { Position = value + Vec2.CompMult(Size, Origin); }
        }
        
        public Vec2 TopRight {
            get { return BottomLeft + Size; }
            set { BottomLeft = value - Size; }
        }
        
        public Vec2 TopLeft {
            get { return BottomLeft + new Vec2(0, Size.Y); }
            set { BottomLeft = value - new Vec2(0, Size.Y); }
        }
        
        public Vec2 BottomRight {
            get { return BottomLeft + new Vec2(Size.X, 0); }
            set { BottomLeft = value - new Vec2(Size.X, 0); }
        }
        
        public Vec2 MidRight {
            get { return BottomLeft + new Vec2(Size.X, Size.Y / 2); }
            set { BottomLeft = value - new Vec2(Size.X, Size.Y / 2); }
        }
        
        public Vec2 MidLeft {
            get { return BottomLeft + new Vec2(0, Size.Y / 2); }
            set { BottomLeft = value - new Vec2(0, Size.Y / 2); }
        }
        
        public Vec2 Center {
            get { return BottomLeft + Size / 2; }
            set { BottomLeft = value - Size / 2; }
        }
        public Vec2 Origin { get; set; }
        public Vec2 Offset { get; set; }
        public Vec2? Anchor { get; set; }

        void UpdatePosition() {
            if (Anchor.HasValue && Parent != null) {
                Position = Parent.BottomLeft + Vec2.CompMult(Parent.Size, Anchor.Value) + Offset;
            }
        }

    }

}
