using Sprint.Characters;
using Sprint.Loader;
using System.Collections.Generic;

namespace Sprint.Levels
{
    internal class Level
    {

        private Room currRoom;
        private List<Room> rooms;
        private GameObjectManager objManager;

        public Level(GameObjectManager objManager)
        {
            this.objManager = objManager;
            rooms = new List<Room>();
        }

        public void Start()
        {
            currRoom = rooms[1];
            currRoom.Enter(Character.Directions.DOWN, objManager);
        }

        public void SwitchRoom(Character.Directions doorDirection, Room room)
        {
            currRoom.Exit(objManager);
            currRoom = room;
            currRoom.Enter(doorDirection, objManager);
        }

        public void AddRoom(Room r)
        {
            rooms.Add(r);
        }

    }
}
