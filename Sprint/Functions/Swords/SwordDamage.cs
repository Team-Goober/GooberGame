using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces;

namespace Sprint.Functions.Swords
{
    internal class SwordDamage : ICommand
    {

        private Character receiver;
        private SwordCollision effector;

        public SwordDamage(ICollidable receiver, ICollidable effector, Vector2 overlap)
        {
            this.receiver = (Character)receiver;
            this.effector = (SwordCollision)effector;
        }

        public void Execute()
        {
            // Damage the character with the sword's damage value
            receiver.TakeDamage(effector.Damage());
        }
    }
}