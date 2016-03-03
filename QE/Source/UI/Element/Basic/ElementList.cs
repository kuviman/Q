using System;

namespace QE.Engine.UI {
    
	public class ElementList : Element {
        
        public ElementList() {
            Spacing = 10;
        }
        
		public bool Horizontal { get; set; }
        
		public double Spacing { get; set; }
        
		public override void Update(double dt) {
			base.Update(dt);
			double w = 0, h = -Spacing;
			foreach (var child in Children) {
				double curW = child.Size.X, curH = child.Size.Y;
				if (Horizontal)
					GUtil.Swap(ref curW, ref curH);
				w = Math.Max(w, curW);
				h += curH + Spacing;
			}
			if (Horizontal)
				GUtil.Swap(ref w, ref h);
			Size = new Vec2(w, h);

			double pos = Horizontal ? BottomLeft.X : TopRight.Y;
			foreach (var child in Children) {
				double nextPos = pos + (Horizontal ? child.Size.X : -child.Size.Y);
				double mid = (pos + nextPos) / 2;
				if (Horizontal)
					child.Position = new Vec2(mid, Center.Y);
				else
					child.Position = new Vec2(Center.X, mid);
				pos = nextPos + (Horizontal ? Spacing : -Spacing);
			}
		}
		
	}

}
