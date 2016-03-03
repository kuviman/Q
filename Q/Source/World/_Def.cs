using System;
using System.Collections.Generic;
using QE;
using QE.EntitySystem;

namespace Q {

    [Serializable]
    class World {
        Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        ESystem esystem = new ESystem();

        public World() {
        }

        public Room this[string room] {
            get { return rooms[room]; }
        }

        public event Action<string> OnNewRoom;
        public void AddRoom(string name) {
            rooms[name] = new Room();
            OnNewRoom?.Invoke(name);
        }

    }

}