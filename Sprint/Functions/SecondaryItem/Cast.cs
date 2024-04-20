using Sprint.Characters;
using Sprint.Interfaces;


namespace Sprint.Functions
{
    internal class Cast : ICommand
    {
        private Player player;

        public Cast(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            this.player.Cast();
        }
    }
}