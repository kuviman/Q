using System;
using OIK = OpenTK.Input.Key;

namespace QE.Engine {

    [Serializable]
    public enum Key : int {

        #region Modifiers
        ShiftLeft = OIK.ShiftLeft,
        LShift = ShiftLeft,
        ShiftRight = OIK.ShiftRight,
        RShift = ShiftRight,
        ControlLeft = OIK.ControlLeft,
        LControl = ControlLeft,
        ControlRight = OIK.ControlRight,
        RControl = ControlRight,
        AltLeft = OIK.AltLeft,
        LAlt = AltLeft,
        AltRight = OIK.AltRight,
        RAlt = AltRight,
        WinLeft = OIK.WinLeft,
        LWin = WinLeft,
        WinRight = OIK.WinRight,
        RWin = WinRight,
        Menu = OIK.Menu,
        #endregion

        #region Function keys
        F1 = OIK.F1,
        F2 = OIK.F2,
        F3 = OIK.F3,
        F4 = OIK.F4,
        F5 = OIK.F5,
        F6 = OIK.F6,
        F7 = OIK.F7,
        F8 = OIK.F8,
        F9 = OIK.F9,
        F10 = OIK.F10,
        F11 = OIK.F11,
        F12 = OIK.F12,
        F13 = OIK.F13,
        F14 = OIK.F14,
        F15 = OIK.F15,
        F16 = OIK.F16,
        F17 = OIK.F17,
        F18 = OIK.F18,
        F19 = OIK.F19,
        F20 = OIK.F20,
        F21 = OIK.F21,
        F22 = OIK.F22,
        F23 = OIK.F23,
        F24 = OIK.F24,
        F25 = OIK.F25,
        F26 = OIK.F26,
        F27 = OIK.F27,
        F28 = OIK.F28,
        F29 = OIK.F29,
        F30 = OIK.F30,
        F31 = OIK.F31,
        F32 = OIK.F32,
        F33 = OIK.F33,
        F34 = OIK.F34,
        F35 = OIK.F35,
        #endregion

        #region Direction arrows
        Up = OIK.Up,
        Down = OIK.Down,
        Left = OIK.Left,
        Right = OIK.Right,
        #endregion

        #region Special keys
        Enter = OIK.Enter,
        Escape = OIK.Escape,
        Space = OIK.Space,
        Tab = OIK.Tab,
        BackSpace = OIK.BackSpace,
        Back = BackSpace,
        Insert = OIK.Insert,
        Delete = OIK.Delete,
        PageUp = OIK.PageUp,
        PageDown = OIK.PageDown,
        Home = OIK.Home,
        End = OIK.End,
        CapsLock = OIK.CapsLock,
        ScrollLock = OIK.ScrollLock,
        PrintScreen = OIK.PrintScreen,
        Pause = OIK.Pause,
        NumLock = OIK.NumLock,
        Clear = OIK.Clear,
        Sleep = OIK.Sleep,
        #endregion

        #region Keypad keys
        Keypad0 = OIK.Keypad0,
        Keypad1 = OIK.Keypad1,
        Keypad2 = OIK.Keypad2,
        Keypad3 = OIK.Keypad3,
        Keypad4 = OIK.Keypad4,
        Keypad5 = OIK.Keypad5,
        Keypad6 = OIK.Keypad6,
        Keypad7 = OIK.Keypad7,
        Keypad8 = OIK.Keypad8,
        Keypad9 = OIK.Keypad9,
        KeypadDivide = OIK.KeypadDivide,
        KeypadMultiply = OIK.KeypadMultiply,
        KeypadSubtract = OIK.KeypadSubtract,
        KeypadMinus = KeypadSubtract,
        KeypadAdd = OIK.KeypadAdd,
        KeypadPlus = KeypadAdd,
        KeypadDecimal = OIK.KeypadDecimal,
        KeypadEnter = OIK.KeypadEnter,
        #endregion

        #region Letters
        A = OIK.A,
        B = OIK.B,
        C = OIK.C,
        D = OIK.D,
        E = OIK.E,
        F = OIK.F,
        G = OIK.G,
        H = OIK.H,
        I = OIK.I,
        J = OIK.J,
        K = OIK.K,
        L = OIK.L,
        M = OIK.M,
        N = OIK.N,
        O = OIK.O,
        P = OIK.P,
        Q = OIK.Q,
        R = OIK.R,
        S = OIK.S,
        T = OIK.T,
        U = OIK.U,
        V = OIK.V,
        W = OIK.W,
        X = OIK.X,
        Y = OIK.Y,
        Z = OIK.Z,
        #endregion

        #region Numbers
        Number0 = OIK.Number0,
        Number1 = OIK.Number1,
        Number2 = OIK.Number2,
        Number3 = OIK.Number3,
        Number4 = OIK.Number4,
        Number5 = OIK.Number5,
        Number6 = OIK.Number6,
        Number7 = OIK.Number7,
        Number8 = OIK.Number8,
        Number9 = OIK.Number9,
        #endregion

        #region Symbols
        Tilde = OIK.Tilde,
        Minus = OIK.Minus,
        Plus = OIK.Plus,
        BracketLeft = OIK.BracketLeft,
        LBracket = BracketLeft,
        BracketRight = OIK.BracketRight,
        RBracket = BracketRight,
        Semicolon = OIK.Semicolon,
        Quote = OIK.Quote,
        Comma = OIK.Comma,
        Period = OIK.Period,
        Slash = OIK.Slash,
        BackSlash = OIK.BackSlash,
        #endregion

    }

    public static class Keyboard {
        public static bool Pressed(this Key key) {
            return App.window.Keyboard[(OIK)key];
        }

    }

}