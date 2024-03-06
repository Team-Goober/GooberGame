using System;
using System.Numerics;

namespace Sprint.Characters
{
    internal class EnemyFactory
    {

        /// <summary>
        /// Builds enemy from string name
        /// </summary>
        /// <param name="name">Name of enemy to make</param>
        /// <param name="position">World position to spawn at</param>
        /// <returns></returns>
        public Character MakeEnemy(String name, Vector2 position)
        {
            // TODO: Implement this function
            // Consider storing enemies in file with reflection, and having enemies load their own sprites
            // This would make dealing with enemies much easier
            return null;
        }

    }
}
