﻿using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;

namespace Sprint.Factory.Door
{
    internal class DoorFactory
    {
        private SpriteLoader spriteLoader;

        public DoorFactory(SpriteLoader spriteLoader)
        {
            this.spriteLoader = spriteLoader;
        }

        /// <summary>
        /// Constructs a door with given parameters
        /// </summary>
        /// <param name="type">Name of Door subclass to use</param>
        /// <param name="spriteFile">File to find sprites in</param>
        /// <param name="spriteLabel">Label for sprite to use</param>
        /// <param name="position">Position in world space for this door</param>
        /// <returns></returns>
        public IDoor MakeDoor(string type, string spriteFile, string spriteLabel, Vector2 position, Vector2 size, int otherSide)
        {
            // TODO: Implement this function
            // Consider storing doors in file with reflection

            ISprite sprite = spriteLoader.BuildSprite(spriteFile, spriteLabel);
            
            switch(type)
            {
                case "open":
                    return new Door(sprite, position, size, otherSide);
                case "wall":
                    return new WallDoor(sprite, position, size, otherSide);
                case "lock":
                    return new LockDoor(sprite, position, size, otherSide);
                default:
                    break;
            }

            return null;

        }

    }
}