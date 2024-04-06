
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Projectile;

namespace Sprint.Functions.SecondaryItem
{
    internal class DissipateProjectile : ICommand
    {

        private DissipatingProjectile receiver;

        public DissipateProjectile(ICollidable receiver, ICollidable effector, Vector2 overlap)
        {
            this.receiver = (DissipatingProjectile)receiver;
        }

        public void Execute()
        {
            receiver.Dissipate();
        }
    }
}
