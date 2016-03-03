using System;

namespace QE.Engine.UI {
    
	public class Button : Element {
        
		public Button(string text, Action action, double size = 20) {
			if (action != null)
                OnClick += action;
			Text = text;
			TextSize = size;
			TextColor = Color.Gray;
			HoveredColor = Color.Sky;
			UnhoveredColor = new Color(0.9, 0.9, 0.9);
			PressOffset = new Vec2(1, -2);
			BorderColor = Color.Gray;
		}
        
		public Color HoveredColor { get; set; }
		public Color UnhoveredColor { get; set; }
        
		public override void Update(double dt) {
			if (Hovered)
				BackgroundColor = HoveredColor;
			else
				BackgroundColor = UnhoveredColor;
			base.Update(dt);
		}
        
		public Vec2 PressOffset { get; set; }
        
		public override void Render() {
			RenderState.Push();
			if (Pressed)
				RenderState.Translate(PressOffset);
			base.Render();
			RenderState.Pop();
		}

	}

}
