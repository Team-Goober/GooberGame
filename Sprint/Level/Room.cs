using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using System.Collections.Generic;
using Sprint.Loader;
using Microsoft.Xna.Framework.Content;
using XMLData;
using Sprint.Characters;

namespace Sprint.Level
{
    internal class Room
    {
        private List<Tiles> tiles;
        private Dictionary<Character.Directions, Door> doors;

        public Room(ContentManager content, SpriteLoader spriteLoader) 
        {
            tiles = new();
            doors = new();
        }

        /// <summary>
        /// Loads this room into the game world
        /// </summary>
        /// <param name="objectManager">GameObjectManager to add objects to</param>
        public void Enter(Character.Directions direction, GameObjectManager objectManager)
        {
            foreach (Tiles tile in tiles)
            {
                objectManager.Add(tile);
            }
        }

        /// <summary>
        /// Removes this room from the game world
        /// </summary>
        /// <param name="objectManager">GameObjectManager to remove objects from</param>
        public void Exit(GameObjectManager objectManager)
        {
            foreach (Tiles tile in tiles)
            {
                objectManager.Remove(tile);
            }
        }

    }
}
