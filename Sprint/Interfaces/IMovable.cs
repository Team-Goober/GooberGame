using Microsoft.Xna.Framework;

namespace Sprint.Interfaces
{
    public interface IMovable
    {
        void Move(Vector2 direction);
        Vector2 GetPosition();
    }
}
