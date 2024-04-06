using System;
using Microsoft.Xna.Framework;

namespace Sprint
{

    public enum DirectionFace
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public static class Directions
    {

        // Constant vector2 for each direction
        public static readonly Vector2 UP = new(0, -1);
        public static readonly Vector2 DOWN = new(0, 1);
        public static readonly Vector2 LEFT = new(-1, 0);
        public static readonly Vector2 RIGHT = new(1, 0);
        public static readonly Vector2 STILL = new(0, 0);


        private static readonly Vector2[] dirIndices = new Vector2[] { UP, RIGHT, DOWN, LEFT }; // Order of directions for all arrays

        // Return vector facing the opposite direction of the given one
        public static Vector2 Opposite(Vector2 direction)
        {
            return -direction;
        }

        public static int GetIndex(Vector2 direction)
        {
            return Array.IndexOf(dirIndices, direction);
        }

        public static Vector2 GetDirectionFromIndex(int idx)
        {
            return dirIndices[idx];
        }

    }
}
