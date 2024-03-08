using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;
using System.Collections.Generic;

namespace Sprint.Factory.Door
{
    internal class DoorFactory
    {
        private SpriteLoader spriteLoader;
        private Dictionary<string, Door> doorType;

        public DoorFactory(SpriteLoader spriteLoader)
        {
            this.spriteLoader = spriteLoader;
            doorType = new Dictionary<string, Door>();
            doorType.Add("TopOpenDoor", new Door());
        }

        /// <summary>
        /// Constructs a door with given parameters
        /// </summary>
        /// <param name="type">Name of Door subclass to use</param>
        /// <param name="spriteFile">File to find sprites in</param>
        /// <param name="spriteLabel">Label for sprite to use</param>
        /// <param name="position">Position in world space for this door</param>
        /// <returns></returns>
        public Door MakeDoor(string type, string spriteFile, string spriteLabel, Vector2 position, Vector2 size)
        {
            // TODO: Implement this function
            // Consider storing doors in file with reflection

            ISprite sprite = spriteLoader.BuildSprite(spriteFile, spriteLabel);



            return doorType[type].Load(sprite, position, size, 1);
        }

    }
}
