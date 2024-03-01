using Microsoft.Xna.Framework;

namespace Sprint.Interfaces
{
    public interface IMovingCollidable : ICollidable
    {
        void Move(Vector2 distance);
    }
}
