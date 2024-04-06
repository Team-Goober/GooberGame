using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Projectile;

namespace Sprint.Functions.SecondaryItem
{
    internal class ProjectileDamage : ICommand 
    {

        private Enemy receiver;
        private SimpleProjectile effector;

        public ProjectileDamage(ICollidable receiver, ICollidable effector, Vector2 overlap)
        {
            this.receiver = (Enemy)receiver;
            this.effector = (SimpleProjectile)effector;
        }

        public void Execute()
        {
            receiver.hp -= effector.dmg;
            receiver.TakeDamage();
        }
    }
}