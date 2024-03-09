using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Collision;
using Sprint.Commands;
using Sprint.Functions.SecondaryItem;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Sprite;
using System;

namespace Sprint.Projectile
{
    internal class BlueArrow : DissipatingProjectile
    {

        private const int SPEED = 300;
        private const int TRAVEL = 400;
        private PlaceSmoke smoke;

        public BlueArrow(ISprite sprite, Vector2 startPos, Vector2 direction, GameObjectManager objManager) :
            base(sprite, startPos, direction, SPEED, TRAVEL, objManager)
        {

        }

        public void SetSmokeCommand(PlaceSmoke smoke)
        {
            this.smoke = smoke;
        }

        public override void Dissipate()
        {
            smoke.Execute();
        }
    }
}
