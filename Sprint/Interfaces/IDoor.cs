using Microsoft.Xna.Framework;

namespace Sprint.Interfaces
{
    internal interface IDoor : IGameObject, ICollidable
    {

        public void SwitchRoom();

        public Vector2 PlayerSpawnPosition();

    }
}
