using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using Sprint.Music.Sfx;
namespace Sprint.Items.Effects
{
    internal class MeleeEffect : IEffect
    {
        private SfxFactory sfxFactory;
        public float damage;

        public MeleeEffect()
        {
            sfxFactory = SfxFactory.GetInstance();
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

        public IEffect Clone()
        {
            return new MeleeEffect() { damage = damage };
        }
    }
}
