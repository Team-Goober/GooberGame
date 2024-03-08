using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Sprint.Collision;

namespace Sprint.Interfaces
{
    public interface ICollidable
    {
        public Rectangle BoundingBox { get; }

        public CollisionTypes[] CollisionType { get; }
    }
}
