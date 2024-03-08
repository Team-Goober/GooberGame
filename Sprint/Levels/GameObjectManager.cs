﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sprint.Levels
{
    public class GameObjectManager
    {

        private List<RoomObjectManager> rooms;
        private RoomObjectManager persistentObjects;
        private int currentRoom = 0;

        public GameObjectManager()
        {
            persistentObjects = new RoomObjectManager();
            rooms = new List<RoomObjectManager>();
        }


        public RoomObjectManager GetRoomObjManager(bool persistent)
        {
            if (persistent)
            {
                return persistentObjects;
            }
            else
            {
                return rooms[currentRoom];
            }
        }

        public void AddRoom(RoomObjectManager room)
        {
            rooms.Add(room);
        }

        public void SwitchRoom(int idx)
        {
            currentRoom = idx;
        }

        public int RoomIndex()
        {
            return currentRoom;
        }

        public int NumRooms()
        {
            return rooms.Count;
        }

        public void ClearRooms()
        {
            rooms.Clear();
        }

        public List<IGameObject> GetObjects()
        {
            return GetRoomObjManager(false).GetObjects().Concat(GetRoomObjManager(true).GetObjects()).ToList();
        }

        public List<ICollidable> GetStatics()
        {
            return GetRoomObjManager(false).GetStatics().Concat(GetRoomObjManager(true).GetStatics()).ToList();
        }

        public List<IMovingCollidable> GetMovers()
        {
            return GetRoomObjManager(false).GetMovers().Concat(GetRoomObjManager(true).GetMovers()).ToList();
        }

        public void Add(IGameObject gameObject, bool persistent = false)
        {
            GetRoomObjManager(persistent).Add(gameObject);
        }

        public void Remove(IGameObject gameObject, bool persistent = false)
        {
            GetRoomObjManager(persistent).Remove(gameObject);
        }

        public void ClearObjects(bool persistent = false)
        {
            GetRoomObjManager(persistent).ClearObjects();
        }

        public void EndCycle()
        {
            GetRoomObjManager(false).EndCycle();
            GetRoomObjManager(true).EndCycle();
        }

    }
}