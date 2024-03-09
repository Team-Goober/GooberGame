
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Projectile;
using System.Diagnostics;

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
