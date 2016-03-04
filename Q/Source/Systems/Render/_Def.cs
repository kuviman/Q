using System;
using QE.EntitySystem;

namespace Q {

    class RenderSystem : ESystem {
        public event Action OnRender;

        public void Render() {
            OnRender?.Invoke();
        }
    }

}