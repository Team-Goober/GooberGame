using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Functions.SecondaryItem;
using Sprint.Music.Sfx;
using Sprint.Characters;

namespace Sprint.Projectile
{
    internal class EnderPearl : DissipatingProjectile
    {

        private const int SPEED = 200;
        private const int TRAVEL = 700;
        private SfxFactory sfxFactory;
        private Character shooter;

        public EnderPearl(ISprite sprite, Vector2 startPos, Vector2 direction, bool isEnemy, Room room, Character shooter) : 
            base(sprite, startPos, direction, SPEED, TRAVEL, isEnemy, room)
        {
            this.shooter = shooter;
            sfxFactory = SfxFactory.GetInstance();
            sfxFactory.PlaySoundEffect("Arrow Shot");
            damage = CharacterConstants.NO_DMG;
        }

        public override void Dissipate()
        {
            sfxFactory.PlaySoundEffect("Run Into Wall");
            // Teleport shooting character to end position
            shooter?.Move(position - shooter.GetPosition());
            // Damage the character on impact
            shooter.TakeDamage(0.5);
            Delete();
        }
    }
}
