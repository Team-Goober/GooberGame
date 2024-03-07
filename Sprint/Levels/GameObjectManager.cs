using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sprint.Levels
{
    public class GameObjectManager
    {

        private List<RoomObjectManager> rooms;
        private int currentRoom = 0;

        public GameObjectManager()
        {
            rooms = new List<RoomObjectManager>();
        }


        public void AddRoom(RoomObjectManager room)
        {
            rooms.Add(room);
        }

        public void SwitchRoom(int idx)
        {
            currentRoom = idx;
        }

        public void ClearRooms()
        {
            rooms.Clear();
        }

        public List<IGameObject> GetObjects()
        {
            return rooms[currentRoom].GetObjects();
        }

        public List<ICollidable> GetStatics()
        {
            return rooms[currentRoom].GetStatics();
        }

        public List<IMovingCollidable> GetMovers()
        {
            return rooms[currentRoom].GetMovers();
        }

        public void Add(IGameObject gameObject)
        {
            rooms[currentRoom].Add(gameObject);
        }

        public void Remove(IGameObject gameObject)
        {
            rooms[currentRoom].Remove(gameObject);
        }

        public void ClearObjects()
        {
            rooms[currentRoom].ClearObjects();
        }

        public void EndCycle()
        {
            rooms[currentRoom].EndCycle();
        }

    }
}
