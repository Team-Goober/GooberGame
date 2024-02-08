using Microsoft.Xna.Framework;

namespace Sprint.Interfaces
{
    internal interface IProjectileFactory
    {

        // Sets the position at which projectiles are created
        void SetStartPosition(Vector2 pos);

        //Sets the direction of the project
        void SetDirection(Vector2 direction);

        // Create whatever shots must be created
        void Create();

    }
}
