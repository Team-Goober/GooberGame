using Microsoft.Xna.Framework;

namespace Sprint.Interfaces
{
    public interface IMovingCollidable : ICollidable
    {
        // Moves the collidable by a given displacement in a single cycle
        void Move(Vector2 distance);
    }
}
