using Microsoft.Xna.Framework;

namespace Sprint.Interfaces
{
    internal interface IProjectile : IGameObject
    {
        void SetPosition(Vector2 pos);

        double DamageAmount();

        Vector2 GetPosition();

        void Create();

        void Delete();


    }
}
