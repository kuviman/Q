using System;

namespace QE.Engine {

    [Serializable]
    public enum MouseButton {
        Left = OpenTK.Input.MouseButton.Left,
        Middle = OpenTK.Input.MouseButton.Middle,
        Right = OpenTK.Input.MouseButton.Right,
    }

    public static class Mouse {

        public static Vec2i Position {
            get { return new Vec2i(App.window.Mouse.X, App.Height - 1 - App.window.Mouse.Y); }
            set {
                System.Windows.Forms.Cursor.Position = App.window.PointToScreen(
                    new System.Drawing.Point(value.X, App.Height - 1 - value.Y));
            }
        }

        static bool _visible = true;
        public static bool Visible {
            get { return _visible; }
            set {
                _visible = value;
                if (_visible)
                    System.Windows.Forms.Cursor.Show();
                else
                    System.Windows.Forms.Cursor.Hide();
            }
        }

        public static bool Pressed(this MouseButton button) {
            return App.window.Mouse[(OpenTK.Input.MouseButton)button];
        }

    }

}