using Microsoft.Xna.Framework;
using Sprint.Collision;

namespace Sprint.Interfaces
{
    public interface ICollidable
    {
        public Rectangle BoundingBox { get; }

        public CollisionTypes[] CollisionType { get; }
    }
}
