using Microsoft.Xna.Framework;
using Sprint.Sprite;
using System;

namespace Sprint.Levels
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
        public Door MakeDoor(string type, string spriteFile, string spriteLabel, Vector2 position, Vector2 size)
        {
            // TODO: Implement this function
            // Consider storing doors in file with reflection
            return null;
        }

    }
}
