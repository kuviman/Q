using System;

namespace QE.Engine.UI {
    
	public class CheckBox : Button {
        
		public CheckBox(double size) : base(null, null, size) {
			OnClick += () => {
				Checked = !Checked;
				OnChanged?.Invoke(Checked);
			};
			Size = new Vec2(size, size);
		}
        
		public bool Checked { get; set; }
        
		public event Action<bool> OnChanged;
        
		protected override void PostRender() {
			base.PostRender();
			if (Checked) {
				RenderState.Push();
				RenderState.Color = BorderColor;
				Draw.Line(BottomLeft, TopRight, BorderWidth);
				Draw.Line(TopLeft, BottomRight, BorderWidth);
				RenderState.Pop();
			}
		}

	}

}
