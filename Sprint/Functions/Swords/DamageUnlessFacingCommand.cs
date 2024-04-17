using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces;

namespace Sprint.Functions.Swords
{
    internal class DamageUnlessFacingCommand : ICommand
    {

        private Player receiver;
        private Vector2 direction;

        public DamageUnlessFacingCommand(ICollidable receiver, ICollidable effector, Vector2 overlap)
        {
            this.receiver = (Player)receiver;
            direction = Vector2.Normalize(overlap);
        }

        public void Execute()
        {
            // Only damage if not facing into the damaging object
            if (direction != receiver.Facing)
            {
                receiver.TakeDamage(0.5);
            }
        }
    }
}