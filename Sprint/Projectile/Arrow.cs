using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Sprite;
using Sprint.Commands;
using System;
using Sprint.Levels;
using Sprint.Collision;
using Sprint.Functions.SecondaryItem;
using System.Transactions;

namespace Sprint.Projectile
{
    internal class Arrow : DissipatingProjectile
    {

        private const int SPEED = 300;
        private const int TRAVEL = 200;
        private PlaceSmoke smoke;

        public Arrow(ISprite sprite, Vector2 startPos, Vector2 direction, GameObjectManager objManager) : 
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
