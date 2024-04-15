using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using Sprint.Music.Sfx;
namespace Sprint.Items.Effects
{
    internal class MeleeEffect : IEffect
    {
        private SfxFactory sfxFactory;
        private float damage;

        public MeleeEffect(float damage)
        {
            sfxFactory = SfxFactory.GetInstance();
            this.damage = damage;
        }

        public void Execute(Player player)
        {
            // Tell player to swing with given damage
            player.Attack(damage);
            // Make swing sound
            sfxFactory.PlaySoundEffect("Sword Swing");
        }

        public void Reverse(Player player)
        {
            // Do nothing
        }
    }
}
