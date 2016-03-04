using QE.EntitySystem;

namespace Q {

    class PlayerGroup : EGroup {
        public PlayerGroup(ESystem esystem, string player) : base(esystem, e => e.Id.StartsWith(player + '#')) { }
    }

}