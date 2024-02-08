using Microsoft.Xna.Framework;

namespace Sprint.Interfaces
{
    internal interface IProjectileFactory
    {

        // Sets the position at which projectiles are created
        void SetStartPosition(Vector2 pos);

        // Create whatever shots must be created
        void Create(Vector2 direction);

    }
}
