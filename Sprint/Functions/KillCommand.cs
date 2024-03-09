using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Projectile;

namespace Sprint.Functions.SecondaryItem
{
    internal class KillCommand : ICommand
    {

        private Character receiver;

        public KillCommand(ICollidable receiver, ICollidable effector, Vector2 overlap)
        {
            this.receiver = (Character)receiver;
        }

        public void Execute()
        {
            receiver.Die();
        }
    }
}