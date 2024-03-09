using Microsoft.Xna.Framework;

namespace Sprint.Interfaces
{
    internal interface IProjectile : IGameObject
    {

        void SetPosition(Vector2 pos);

        Vector2 GetPosition();

        void Create();

        void Delete();

    }
}
