using System;

namespace QE.Engine.UI {
    
	public class Scale : Element {
        
		public Scale(double length, double size) {
			Size = new Vec2(length, size);
			BorderColor = Color.Gray;
			BackgroundColor = new Color(0.9, 0.9, 0.9);
		}

		double _position = 0.5;
        
		public double Value {
			get { return _position; }
			set { _position = Math.Min(1, Math.Max(0, value)); }
		}
        
		public override void Render() {
			base.Render();
			RenderState.Push();
			RenderState.Color = BorderColor;
			Draw.Line(MidLeft, MidRight, BorderWidth / 2);
			RenderState.Translate(BottomLeft + new Vec2(Value * Size.X, 0));
			RenderState.Translate(0, Size.Y / 2);
			if (Pressed)
				RenderState.Scale(0.8);
			RenderState.Origin(0, Size.Y / 2);
			RenderState.Color = Hovered ? new Color(0.8, 0.8, 1) : new Color(0.8, 0.8, 0.8);
			double wid = 5;
			Draw.Rect(-wid, 0, wid, Size.Y);
			RenderState.Color = new Color(0.5, 0.5, 0.5);
			Draw.Frame(-wid, 0, wid, Size.Y, BorderWidth);
			RenderState.Pop();
		}

		double lastPos = 0.5;
        
		public override void MouseMove(Vec2 position) {
			base.MouseMove(position);
			lastPos = (position.X - BottomLeft.X) / Size.X;
			if (Pressed) {
				Value = lastPos;
				if (OnChanging != null)
					OnChanging.Invoke(Value);
			}
		}

		public override void Press() {
			base.Press();
			Value = lastPos;
			if (OnChanging != null) {
				OnChanging.Invoke(Value);
			}
		}
        
		public override void Release() {
			base.Release();
			if (OnChanged != null)
				OnChanged.Invoke(Value);
		}
        
		public event Action<double> OnChanged;
        
		public event Action<double> OnChanging;

	}

}
