using System;
using System.Collections.Generic;
using QE;
using QE.EntitySystem;

namespace Q {

    [Serializable]
    class World {
        Dictionary<string, Room> rooms = new Dictionary<string, Room>();

        public World() {
        }

        public Room this[string room] {
            get {
                if (!rooms.ContainsKey(room))
                    return null;
                return rooms[room];
            }
        }

        public event Action<string> OnNewRoom;
        public void AddRoom(string name) {
            rooms[name] = new Room();
            OnNewRoom?.Invoke(name);
        }

    }

}