using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Items;
using System;
using System.Collections.Generic;

namespace Sprint.Levels
{
    internal class Room
    {

        private SceneObjectManager objectManager; // Manages all objects current active in room
        private List<IDoor> doors; // List of doors at start of room
        private List<Character> npcs; // List of enemies at start of room
        private List<Item> items; // List of items at start of room
        private bool hidden; // Whether this room should stay hidden on the map

        public Room(bool hidden)
        {
            objectManager = new SceneObjectManager();
            this.doors = new();
            this.npcs = new();
            this.items = new();
            this.hidden = hidden;
        }

        public List<IDoor> GetDoors()
        {
            return doors;
        }

        public List<Character> GetNpcs()
        {
            return npcs;
        }

        public List<Item> GetItems()
        {
            return items;
        }

        public SceneObjectManager GetScene()
        {
            return objectManager;
        }
        
        public bool IsHidden()
        {
            return hidden;
        }

    }
}
