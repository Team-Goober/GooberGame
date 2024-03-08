
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Projectile;
using System.Diagnostics;
using static Sprint.Characters.Character;

namespace Sprint.Functions.SecondaryItem
{
    internal class DissipateProjectile : ICommand
    {

        private DissipatingProjectile receiver;

        public DissipateProjectile(IMovingCollidable receiver, Vector2 overlap)
        {
            this.receiver = (DissipatingProjectile)receiver;
        }

        public void Execute()
        {
            receiver.Dissipate();
        }
    }
}
