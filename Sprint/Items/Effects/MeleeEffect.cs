using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using Sprint.Music.Sfx;
namespace Sprint.Items.Effects
{
    internal class MeleeEffect : IEffect
    {
        private SfxFactory sfxFactory;

        public MeleeEffect()
        {
            sfxFactory = SfxFactory.GetInstance();
        }

        public void Execute(Player player)
        {
            player.Attack();
            sfxFactory.PlaySoundEffect("Sword Swing");
        }
    }
}
