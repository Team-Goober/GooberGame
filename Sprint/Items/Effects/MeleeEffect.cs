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
            player.Attack(damage);
            sfxFactory.PlaySoundEffect("Sword Swing");
        }
    }
}
