using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Projectile;

namespace Sprint.Functions.SecondaryItem
{
    internal class ProjectileDamage : ICommand 
    {

        private Character receiver;
        private SimpleProjectile effector;

        public ProjectileDamage(ICollidable receiver, ICollidable effector, Vector2 overlap)
        {
            this.receiver = (Character)receiver;
            this.effector = (SimpleProjectile)effector;
        }

        public void Execute()
        {
            // Make projectile deal with a collision by causing damage
            effector.Hit(receiver);
        }
    }
}