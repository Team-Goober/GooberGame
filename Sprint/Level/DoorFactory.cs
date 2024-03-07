using Microsoft.Xna.Framework;
using System;

namespace Sprint.Level
{
    internal class DoorFactory
    {

        /// <summary>
        /// Constructs a door with given parameters
        /// </summary>
        /// <param name="type">Name of Door subclass to use</param>
        /// <param name="spriteFile">File to find sprites in</param>
        /// <param name="spriteLabel">Label for sprite to use</param>
        /// <param name="position">Position in world space for this door</param>
        /// <returns></returns>
        public Door MakeDoor(String type, String spriteFile, String spriteLabel, Vector2 position)
        {
            // TODO: Implement this function
            // Consider storing doors in file with reflection
            return null;
        }

    }
}
