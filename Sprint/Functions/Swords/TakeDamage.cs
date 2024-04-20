using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces;

namespace Sprint.Functions.Swords
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
            receiver.TakeDamage(0.5);
        }
    }
}