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
using Sprint.Music.Sfx;

namespace Sprint.Projectile
{
    internal class Arrow : DissipatingProjectile
    {

        private const int SPEED = 300;
        private const int TRAVEL = 200;
        private PlaceSmoke smoke;
        private SfxFactory sfxFactory;

        public Arrow(ISprite sprite, Vector2 startPos, Vector2 direction, bool isEnemy, Room room) : 
            base(sprite, startPos, direction, SPEED, TRAVEL, isEnemy, room)
        {
            sfxFactory = SfxFactory.GetInstance();
            sfxFactory.PlaySoundEffect("Arrow Shot");
            damage = 1;
        }

        public void SetSmokeCommand(PlaceSmoke smoke)
        {
            this.smoke = smoke;
        }

        public override void Dissipate()
        {
            sfxFactory.PlaySoundEffect("Run Into Wall");
            smoke.Execute();
        }
    }
}
