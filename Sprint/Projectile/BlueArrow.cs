using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Functions.SecondaryItem;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Music.Sfx;

namespace Sprint.Projectile
{
    internal class BlueArrow : DissipatingProjectile
    {

        private const int SPEED = 300;
        private const int TRAVEL = 400;
        private PlaceSmoke smoke;
        private SfxFactory sfxFactory;

        public BlueArrow(ISprite sprite, Vector2 startPos, Vector2 direction, bool isEnemy, Room room) :
            base(sprite, startPos, direction, SPEED, TRAVEL, isEnemy, room)
        {
            sfxFactory = SfxFactory.GetInstance();
            sfxFactory.PlaySoundEffect("Arrow Shot");
            damage = CharacterConstants.MID_DMG;
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
