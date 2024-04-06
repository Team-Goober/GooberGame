using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Projectile;

namespace Sprint.Functions.SecondaryItem
{
    internal class TakeDamage : ICommand
    {

        private Character receiver;

        public TakeDamage(ICollidable receiver, ICollidable effector, Vector2 overlap)
        {
            this.receiver = (Character)receiver;
        }

        public void Execute()
        {
            receiver.TakeDamage();
        }
    }
}