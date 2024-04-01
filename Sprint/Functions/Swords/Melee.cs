using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Music.Sfx;


namespace Sprint.Commands
{
    internal class Melee : ICommand
    {
        private Player player;
        private SfxFactory sfxFactory;

        public Melee(Player player)
        {
            this.player = player;
            sfxFactory = new SfxFactory();
        }

        public void Execute()
        {
            this.player.Attack();
            sfxFactory.PlaySoundEffect("Sword Swing");
        }
    }
}