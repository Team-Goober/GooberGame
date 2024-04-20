using Microsoft.Xna.Framework;
using Sprint.Characters;

namespace Sprint.Interfaces
{
    internal interface IProjectile : IGameObject
    {
        void SetPosition(Vector2 pos);

        void Hit(Character subject);

        Vector2 GetPosition();

        void Create();

        void Delete();


    }
}
