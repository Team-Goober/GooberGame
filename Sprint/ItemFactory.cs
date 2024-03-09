using System;
using System.Numerics;
using Sprint.Levels;

namespace Sprint.Characters
{
    internal class ItemFactory
    {

        /// <summary>
        /// Builds item from string name
        /// </summary>
        /// <param name="name">Name of item to make</param>
        /// <param name="position">World position to spawn at</param>
        /// <returns></returns>
        public Item MakeItem(String name, Vector2 position)
        {
            // TODO: Implement this function
            // Consider storing items in file with reflection, and having items load their own sprites
            // This would make dealing with items much easier
            return null;
        }

    }
}
