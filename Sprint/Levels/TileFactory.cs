using Microsoft.Xna.Framework;
using System;

namespace Sprint.Level
{
    internal class TileFactory
    {

        /// <summary>
        /// Constructs a tile with given parameters
        /// </summary>
        /// <param name="type">Name of Tile subclass to use</param>
        /// <param name="spriteFile">File to find sprites in</param>
        /// <param name="spriteLabel">Label for sprite to use</param>
        /// <param name="position">Position in world space for this tile</param>
        /// <returns></returns>
        public Tiles MakeTile(String type, String spriteFile, String spriteLabel, Vector2 position)
        {
            // TODO: Implement this function
            // Consider storing tiles in file with reflection
            return null;
        }

    }
}
