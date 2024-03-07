
using Sprint.Characters;
using Sprint.Loader;

namespace Sprint.Level
{
    internal class Level
    {

        private Room currRoom;
        private Room firstRoom;
        private GameObjectManager objManager;

        public Level(GameObjectManager objManager) {
            this.objManager = objManager;
        }

        public void Start()
        {
            currRoom = firstRoom;
            currRoom.Enter(Character.Directions.DOWN, objManager);
        }

        public void SwitchRoom(Character.Directions doorDirection, Room room)
        {
            currRoom.Exit(objManager);
            currRoom = room;
            currRoom.Enter(doorDirection, objManager);
        }

    }
}
