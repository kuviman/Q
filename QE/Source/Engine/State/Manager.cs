using System;
using System.Collections.Generic;

namespace QE.Engine {

    partial class State {
        
        public State NextState { private get; set; }

        Stack<State> StateStack = new Stack<State>();
        
        public void PushState(State state) {
            StateStack.Push(state);
        }
        
        public class Manager : State {
            
            public State CurrentState { get; private set; }
            
            public Manager(params State[] states) {
                for (int i = states.Length - 1; i >= 0; i--)
                    ManagedStack.Push(states[i]);
            }
            
            public override void KeyDown(Key key) {
                base.KeyDown(key);
                if (CurrentState != null)
                    CurrentState.KeyDown(key);
            }
            
            public override void KeyRepeat(Key key) {
                base.KeyRepeat(key);
                if (CurrentState != null)
                    CurrentState.KeyRepeat(key);
            }
            
            public override void KeyUp(Key key) {
                base.KeyUp(key);
                if (CurrentState != null)
                    CurrentState.KeyUp(key);
            }
            
            public override void CharInput(char c) {
                base.CharInput(c);
                if (CurrentState != null)
                    CurrentState.CharInput(c);
            }
            
            public override void MouseDown(MouseButton button, Vec2 position) {
                base.MouseDown(button, position);
                if (CurrentState != null)
                    CurrentState.MouseDown(button, position);
            }
            
            public override void MouseUp(MouseButton button, Vec2 position) {
                base.MouseUp(button, position);
                if (CurrentState != null)
                    CurrentState.MouseUp(button, position);
            }
            
            public override void MouseWheel(double delta) {
                base.MouseWheel(delta);
                if (CurrentState != null)
                    CurrentState.MouseWheel(delta);
            }
            
            public override void MouseMove(Vec2 position) {
                base.MouseMove(position);
                if (CurrentState != null)
                    CurrentState.MouseMove(position);
            }
            
            public override void Render() {
                base.Render();
                if (CurrentState != null)
                    CurrentState.Render();
            }

            State prevState = null;
            
            public override void Update(double dt) {
                base.Update(dt);
                while (ManagedStack.Count != 0) {
                    if (ManagedStack.Peek() == null || ManagedStack.Peek().Closed)
                        ManagedStack.Pop();
                    else if (ManagedStack.Peek().NextState != null)
                        ManagedStack.Push(ManagedStack.Pop().NextState);
                    else break;
                }
                CurrentState = ManagedStack.Count == 0 ? null : ManagedStack.Peek();
                if (CurrentState != null) {
                    CurrentState.Update(dt);
                    while (CurrentState.StateStack.Count != 0)
                        PushState(CurrentState.StateStack.Pop());
                } else {
                    Close();
                }

                if (CurrentState != prevState) {
                    StateChanged();
                    prevState = CurrentState;
                }
            }
            
            public virtual void StateChanged() { }
            
            public new State NextState {
                set {
                    if (ManagedStack.Count != 0)
                        ManagedStack.Pop();
                    PushState(value);
                }
            }
            
            public new void PushState(State state) {
                ManagedStack.Push(state);
            }

            Stack<State> ManagedStack = new Stack<State>();

        }

    }

}