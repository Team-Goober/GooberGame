using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Sprint
{
    public static class Directions
    {

        // Constant vector2 for each direction
        public static readonly Vector2 UP = new(0, -1);
        public static readonly Vector2 DOWN = new(0, 1);
        public static readonly Vector2 LEFT = new(-1, 0);
        public static readonly Vector2 RIGHT = new(1, 0);
        public static readonly Vector2 STILL = new(0, 0);


        // Return vector facing the opposite direction of the given one
        public static Vector2 Opposite(Vector2 direction)
        {
            return -direction;
        }

    }
}
